﻿using Aesob.Web.Core.Internal.Configuration;
using Aesob.Web.Core.Internal.Services;
using Aesob.Web.Library;
using Aesob.Web.Library.Configuration;
using Aesob.Web.Library.Extensions;
using Aesob.Web.Library.Service;
using System.Reflection;

namespace Aesob.Web.Core.Internal
{
    internal class ServiceManager
    {
        private const int _serviceUpdateInterval = (int)(0.1f * 1000);

        internal static ServiceManager Instance { get; private set; }

        private static bool _isRunning;

        private Dictionary<Type, IAesobService> _serviceInstances;
        private DateTime _lastUpdateTime;

        private IAesobConfigurationManager _configurationManager;
        private IAesobServiceProvider _serviceProvider;

        internal ServiceManager()
        {
            _serviceInstances = new Dictionary<Type, IAesobService>();

            Instance = this;
        }

        internal IAesobService GetService<T>() where T : IAesobService
        {
            if(_serviceInstances.TryGetValue(typeof(T), out var service))
            {
                return service;
            }

            return null;
        }

        internal void Start()
        {
            _serviceProvider = new AesobServiceProvider();
            _configurationManager = new AesobConfigurationManager();
            _serviceInstances.Clear();

            Debug.Assert(_serviceProvider != null, "Service Provider can't be null");
            Debug.Assert(_configurationManager != null, "Configuration Manager can't be null");

            try
            {
                var serviceTypes = _serviceProvider.CollectAvailableServiceTypes().ToList();
                _serviceInstances = CreateServices(serviceTypes.ToList());

                ConfigureServices(_serviceInstances.Values.ToList());

                foreach (var serviceKvp in _serviceInstances)
                {
                    Debug.Print("Starting Service: " + serviceKvp.Key.Name);
                    serviceKvp.Value.Start();
                }

                _isRunning = true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        internal async Task Update()
        {
            _lastUpdateTime = DateTime.Now;

            while (_isRunning)
            {
                await Task.Delay(_serviceUpdateInterval);

                var curTime = DateTime.Now;
                var timeDifference = curTime - _lastUpdateTime;

                var deltaTime = (float)timeDifference.TotalMilliseconds / 1000f;

                foreach (var service in _serviceInstances)
                {
                    service.Value.Update(deltaTime);
                }

                _lastUpdateTime = curTime;
            }
        }

        internal bool Stop()
        {
            try
            {
                _isRunning = false;

                foreach (var service in _serviceInstances)
                {
                    service.Value.Stop();
                }

                _serviceInstances.Clear();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private Dictionary<Type, IAesobService> CreateServices(List<Type> serviceTypes)
        {
            Debug.Print("Creating services...");

            var services = new Dictionary<Type, IAesobService>();

            var dependencyDatas = CollectServiceDependencyDatas(serviceTypes);
            dependencyDatas.Sort(new ServiceDependencyComparer());

            foreach (var data in dependencyDatas.Values)
            {
                var dependencies = new List<IAesobService>();

                foreach (var depType in data.DependencyParameters)
                {
                    IAesobService depService;

                    services.TryGetValue(depType, out depService);

                    dependencies.Add(depService);
                }

                var serviceInstance = data.Constructor?.Invoke(dependencies.ToArray()) as IAesobService;
                if (serviceInstance != null)
                {
                    services.Add(data.ServiceType, serviceInstance);
                }
            }

            return services;
        }

        private Dictionary<Type, ServiceDependencyData> CollectServiceDependencyDatas(List<Type> serviceTypes)
        {
            Dictionary<Type, ServiceDependencyData> dependencyDatas = new Dictionary<Type, ServiceDependencyData>();

            foreach (var type in serviceTypes)
            {
                if (!type.IsAesobService())
                {
                    Debug.FailedAssert("Type is not an Aesob Service: " + type.Name);
                    continue;
                }

                var data = ServiceDependencyData.CreateFrom(type);

                foreach (var dependedType in data.DependencyParameters)
                {
                    if (dependencyDatas.TryGetValue(dependedType, out var depData))
                    {
                        if (depData.DependencyParameters.Contains(type))
                        {
                            Debug.FailedAssert("Circular dependency between: " + type.Name + " and " + dependedType.Name);
                        }
                    }
                }

                dependencyDatas.Add(type, data);
            }

            return dependencyDatas;
        }

        private void ConfigureServices(List<IAesobService> services)
        {
            Debug.Print("Configuring services...");
            _configurationManager.ConfigureServices(services);
        }

        private class ServiceDependencyData
        {
            public Type ServiceType { get; private set; }
            public ConstructorInfo Constructor { get; private set; }
            public List<Type> DependencyParameters { get; private set; }

            public static ServiceDependencyData CreateFrom(Type serviceType)
            {
                var constructor = serviceType.GetConstructors().FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();
                    return parameters.Length == 0 || parameters.All(c => c.ParameterType.IsAesobService());
                });

                Debug.Assert(constructor != null, "No suitable constructors found for service: " + serviceType.Name);

                var data = new ServiceDependencyData();

                data.ServiceType = serviceType;
                data.Constructor = constructor;
                data.DependencyParameters = CreateDependencyParameters(constructor);

                return data;
            }

            private static List<Type> CreateDependencyParameters(ConstructorInfo constructor)
            {
                List<Type> dependencies = new List<Type>();

                if (constructor != null)
                {
                    foreach (var parameter in constructor.GetParameters())
                    {
                        if (parameter.ParameterType.IsAesobService())
                        {
                            dependencies.Add(parameter.ParameterType);
                        }
                        else
                        {
                            Debug.FailedAssert("All parameter types are not IAesobService in constructor: " + constructor.DeclaringType.Name);
                        }
                    }
                }

                return dependencies;
            }
        }

        private class ServiceDependencyComparer : IComparer<ServiceDependencyData>
        {
            public int Compare(ServiceDependencyData x, ServiceDependencyData y)
            {
                if (x.DependencyParameters.Contains(y.ServiceType))
                {
                    return 1;
                }
                else if (y.DependencyParameters.Contains(x.ServiceType))
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}

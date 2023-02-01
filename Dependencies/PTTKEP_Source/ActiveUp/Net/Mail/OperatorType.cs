using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public enum OperatorType
	{
		GreaterThan,
		GreaterThanEqual,
		LessThan,
		LessThanEqual,
		Equal,
		NotEqual,
		Exists,
		NotExists
	}
}

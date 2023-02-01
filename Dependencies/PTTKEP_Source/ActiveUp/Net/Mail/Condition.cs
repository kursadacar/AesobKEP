using System;
using System.Globalization;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Condition
	{
		private string _regionID;

		private string _field;

		private string _value;

		private string _nulltext;

		private OperatorType _operator;

		private bool _caseSensitive;

		private bool _match;

		public string NullText
		{
			get
			{
				return _nulltext;
			}
			set
			{
				_nulltext = value;
			}
		}

		public bool CaseSensitive
		{
			get
			{
				return _caseSensitive;
			}
			set
			{
				_caseSensitive = value;
			}
		}

		public bool Match
		{
			get
			{
				return _match;
			}
			set
			{
				_match = value;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public OperatorType Operator
		{
			get
			{
				return _operator;
			}
			set
			{
				_operator = value;
				if (_operator == OperatorType.NotExists)
				{
					_match = true;
				}
			}
		}

		public string RegionID
		{
			get
			{
				return _regionID;
			}
			set
			{
				_regionID = value;
			}
		}

		public string Field
		{
			get
			{
				return _field;
			}
			set
			{
				_field = value;
			}
		}

		public Condition()
		{
			RegionID = string.Empty;
			Field = string.Empty;
			Operator = OperatorType.Equal;
			CaseSensitive = true;
			Value = null;
		}

		public Condition(string regionid, string field, string aValue)
		{
			RegionID = regionid;
			Field = field;
			Operator = OperatorType.Equal;
			CaseSensitive = true;
			Value = aValue;
		}

		public Condition(string regionid, string field, string aValue, OperatorType aOperator, bool casesensitive)
		{
			RegionID = regionid;
			Field = field;
			Operator = aOperator;
			CaseSensitive = casesensitive;
			Value = aValue;
		}

		public void Validate(object aValue)
		{
			try
			{
				if (Value == null || Match)
				{
					return;
				}
				switch (Operator)
				{
				case OperatorType.GreaterThan:
					if (aValue == null)
					{
						aValue = 0;
					}
					if (IsNumeric((string)aValue) && IsNumeric(Value))
					{
						if (Convert.ToDouble(aValue) < Convert.ToDouble(Value))
						{
							Match = true;
						}
					}
					else if (aValue.ToString().Length < Value.Length)
					{
						Match = true;
					}
					break;
				case OperatorType.GreaterThanEqual:
					if (aValue == null)
					{
						aValue = 0;
					}
					if (IsNumeric((string)aValue) && IsNumeric(Value))
					{
						if (Convert.ToDouble(aValue) <= Convert.ToDouble(Value))
						{
							Match = true;
						}
					}
					else if (aValue.ToString().Length <= Value.Length)
					{
						Match = true;
					}
					break;
				case OperatorType.LessThan:
					if (aValue == null)
					{
						aValue = 0;
					}
					if (IsNumeric((string)aValue) && IsNumeric(Value))
					{
						if (Convert.ToDouble(aValue) > Convert.ToDouble(Value))
						{
							Match = true;
						}
					}
					else if (aValue.ToString().Length > Value.Length)
					{
						Match = true;
					}
					break;
				case OperatorType.LessThanEqual:
					if (aValue == null)
					{
						aValue = 0;
					}
					if (IsNumeric((string)aValue) && IsNumeric(Value))
					{
						if (Convert.ToDouble(aValue) >= Convert.ToDouble(Value))
						{
							Match = true;
						}
					}
					else if (aValue.ToString().Length >= Value.Length)
					{
						Match = true;
					}
					break;
				case OperatorType.Equal:
					if (aValue == null)
					{
						aValue = "";
					}
					if (CaseSensitive)
					{
						if (aValue.ToString() == Value)
						{
							Match = true;
						}
					}
					else if (aValue.ToString().ToUpper() == Value.ToUpper())
					{
						Match = true;
					}
					break;
				case OperatorType.NotEqual:
					if (aValue == null)
					{
						aValue = "";
					}
					if (CaseSensitive)
					{
						if (aValue.ToString() != Value)
						{
							Match = true;
						}
					}
					else if (aValue.ToString().ToUpper() != Value.ToUpper())
					{
						Match = true;
					}
					break;
				case OperatorType.Exists:
					Match = true;
					break;
				case OperatorType.NotExists:
					Match = false;
					break;
				}
			}
			catch
			{
			}
		}

		public bool IsNumeric(object expression)
		{
			double result;
			return double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);
		}
	}
}

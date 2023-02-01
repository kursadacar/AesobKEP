using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Templater
	{
		private Logger _logger;

		private Message _message;

		private FieldFormatCollection _fieldsFormats;

		private ConditionalCollection _conditions;

		private RegionCollection _regions;

		private ServerCollection _smtpServers;

		private ListTemplateCollection _listTemplates;

		public Logger Logger
		{
			get
			{
				return _logger ?? (_logger = new Logger());
			}
			set
			{
				_logger = value;
			}
		}

		public Message Message
		{
			get
			{
				return _message ?? (_message = new Message());
			}
			set
			{
				_message = value;
			}
		}

		public RegionCollection Regions
		{
			get
			{
				return _regions ?? (_regions = new RegionCollection());
			}
			set
			{
				_regions = value;
			}
		}

		public ConditionalCollection Conditions
		{
			get
			{
				return _conditions ?? (_conditions = new ConditionalCollection());
			}
			set
			{
				_conditions = value;
			}
		}

		public FieldFormatCollection FieldsFormats
		{
			get
			{
				return _fieldsFormats ?? (_fieldsFormats = new FieldFormatCollection());
			}
			set
			{
				_fieldsFormats = value;
			}
		}

		public ServerCollection SmtpServers
		{
			get
			{
				return _smtpServers ?? (_smtpServers = new ServerCollection());
			}
			set
			{
				_smtpServers = value;
			}
		}

		public ListTemplateCollection ListTemplates
		{
			get
			{
				return _listTemplates ?? (_listTemplates = new ListTemplateCollection());
			}
			set
			{
				_listTemplates = value;
			}
		}

		public Templater()
		{
		}

		public Templater(string templateFile)
		{
			LoadTemplate(templateFile);
		}

		public void LoadTemplate(string filename)
		{
			Logger.AddEntry("Loading template " + filename + ".", 2);
			string text = LoadFileContent(filename);
			Logger.AddEntry("Template length: " + text.Length + " bytes.", 1);
			if (text.Length > 0)
			{
				ProcessXmlTemplate(text);
				return;
			}
			throw new Exception("The specified file is empty or not valid.");
		}

		public void LoadTemplateFromString(string content)
		{
			Logger.AddEntry("Loading template from string.", 2);
			Logger.AddEntry("Template length: " + content.Length + " bytes.", 1);
			ProcessXmlTemplate(content);
		}

		private void ProcessXmlTemplate(string content)
		{
			Logger.AddEntry("Processing the XML template.", 1);
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(content));
			string text = string.Empty;
			while (xmlTextReader.Read())
			{
				switch (xmlTextReader.NodeType)
				{
				case XmlNodeType.Element:
					text = xmlTextReader.Name;
					Logger.AddEntry("New element found: " + text + ".", 0);
					switch (text.ToUpper())
					{
					case "MESSAGE":
						if (xmlTextReader.GetAttribute("PRIORITY") != null && xmlTextReader.GetAttribute("PRIORITY") != string.Empty)
						{
							Message.Priority = (MessagePriority)Enum.Parse(typeof(MessagePriority), xmlTextReader.GetAttribute("PRIORITY"), true);
						}
						else if (xmlTextReader.GetAttribute("priority") != null && xmlTextReader.GetAttribute("priority") != string.Empty)
						{
							Message.Priority = (MessagePriority)Enum.Parse(typeof(MessagePriority), xmlTextReader.GetAttribute("priority"), true);
						}
						break;
					case "FIELDFORMAT":
					{
						if (!xmlTextReader.HasAttributes)
						{
							break;
						}
						Logger.AddEntry("Element has attributes.", 0);
						FieldFormat fieldFormat = new FieldFormat();
						if (xmlTextReader.GetAttribute("NAME") != null && xmlTextReader.GetAttribute("NAME") != string.Empty)
						{
							fieldFormat.Name = xmlTextReader.GetAttribute("NAME");
							Logger.AddEntry("Attribute NAME: " + fieldFormat.Name, 0);
						}
						else if (xmlTextReader.GetAttribute("name") != null && xmlTextReader.GetAttribute("name") != string.Empty)
						{
							fieldFormat.Name = xmlTextReader.GetAttribute("name");
							Logger.AddEntry("Attribute name: " + fieldFormat.Name, 0);
						}
						if (xmlTextReader.GetAttribute("FORMAT") != null && xmlTextReader.GetAttribute("FORMAT") != string.Empty)
						{
							fieldFormat.Format = xmlTextReader.GetAttribute("FORMAT");
							Logger.AddEntry("Attribute FORMAT: " + fieldFormat.Format, 0);
						}
						else if (xmlTextReader.GetAttribute("format") != null && xmlTextReader.GetAttribute("format") != string.Empty)
						{
							fieldFormat.Format = xmlTextReader.GetAttribute("format");
							Logger.AddEntry("Attribute format: " + fieldFormat.Format, 0);
						}
						if (xmlTextReader.GetAttribute("PADDINGDIR") != null && xmlTextReader.GetAttribute("PADDINGDIR") != string.Empty)
						{
							fieldFormat.PaddingDir = ((!(xmlTextReader.GetAttribute("PADDINGDIR").ToUpper() == "LEFT")) ? PaddingDirection.Right : PaddingDirection.Left);
							Logger.AddEntry("Attribute PADDINGDIR: " + xmlTextReader.GetAttribute("PADDINGDIR"), 0);
						}
						else if (xmlTextReader.GetAttribute("paddingdir") != null && xmlTextReader.GetAttribute("paddingdir") != string.Empty)
						{
							fieldFormat.PaddingDir = ((!(xmlTextReader.GetAttribute("paddingdir").ToUpper() == "left")) ? PaddingDirection.Right : PaddingDirection.Left);
							Logger.AddEntry("Attribute paddingdir: " + xmlTextReader.GetAttribute("paddingdir"), 0);
						}
						if (xmlTextReader.GetAttribute("TOTALWIDTH") != null && xmlTextReader.GetAttribute("TOTALWIDTH") != string.Empty)
						{
							try
							{
								fieldFormat.TotalWidth = Convert.ToInt16(xmlTextReader.GetAttribute("TOTALWIDTH"));
							}
							catch
							{
								throw new Exception("Specified Total Width is not a valid number.");
							}
							Logger.AddEntry("Attribute TOTALWIDTH: " + fieldFormat.TotalWidth, 0);
						}
						else if (xmlTextReader.GetAttribute("totalwidth") != null && xmlTextReader.GetAttribute("totalwidth") != string.Empty)
						{
							try
							{
								fieldFormat.TotalWidth = Convert.ToInt16(xmlTextReader.GetAttribute("totalwidth"));
							}
							catch
							{
								throw new Exception("Specified Total Width is not a valid number.");
							}
							Logger.AddEntry("Attribute totalwidth: " + fieldFormat.TotalWidth, 0);
						}
						if (xmlTextReader.GetAttribute("PADDINGCHAR") != null && xmlTextReader.GetAttribute("PADDINGCHAR") != string.Empty)
						{
							fieldFormat.PaddingChar = Convert.ToChar(xmlTextReader.GetAttribute("PADDINGCHAR").Substring(0, 1));
							Logger.AddEntry("Attribute PADDINGCHAR: '" + fieldFormat.PaddingChar + "'", 0);
						}
						else if (xmlTextReader.GetAttribute("paddingchar") != null && xmlTextReader.GetAttribute("paddingchar") != string.Empty)
						{
							fieldFormat.PaddingChar = Convert.ToChar(xmlTextReader.GetAttribute("paddingchar").Substring(0, 1));
							Logger.AddEntry("Attribute paddingchar: '" + fieldFormat.PaddingChar + "'", 0);
						}
						FieldsFormats.Add(fieldFormat);
						break;
					}
					case "FROM":
					case "TO":
					case "CC":
					case "BCC":
					{
						if (!xmlTextReader.HasAttributes)
						{
							break;
						}
						Address address = new Address();
						if (xmlTextReader.GetAttribute("NAME") != null && xmlTextReader.GetAttribute("NAME") != string.Empty)
						{
							address.Name = xmlTextReader.GetAttribute("NAME");
						}
						else if (xmlTextReader.GetAttribute("name") != null && xmlTextReader.GetAttribute("name") != string.Empty)
						{
							address.Name = xmlTextReader.GetAttribute("name");
						}
						if (xmlTextReader.GetAttribute("EMAIL") != null && xmlTextReader.GetAttribute("EMAIL") != string.Empty)
						{
							address.Email = xmlTextReader.GetAttribute("EMAIL");
						}
						else if (xmlTextReader.GetAttribute("email") != null && xmlTextReader.GetAttribute("email") != string.Empty)
						{
							address.Email = xmlTextReader.GetAttribute("email");
						}
						if (text.ToUpper() == "FROM")
						{
							if (xmlTextReader.GetAttribute("REPLYNAME") != null && xmlTextReader.GetAttribute("REPLYNAME") != string.Empty)
							{
								InitReplyTo();
								Message.ReplyTo.Name = xmlTextReader.GetAttribute("REPLYNAME");
							}
							else if (xmlTextReader.GetAttribute("replyname") != null && xmlTextReader.GetAttribute("replyname") != string.Empty)
							{
								InitReplyTo();
								Message.ReplyTo.Name = xmlTextReader.GetAttribute("replyname");
							}
							if (xmlTextReader.GetAttribute("REPLYEMAIL") != null && xmlTextReader.GetAttribute("REPLYEMAIL") != string.Empty)
							{
								InitReplyTo();
								Message.ReplyTo.Email = xmlTextReader.GetAttribute("REPLYEMAIL");
							}
							else if (xmlTextReader.GetAttribute("replyemail") != null && xmlTextReader.GetAttribute("replyemail") != string.Empty)
							{
								InitReplyTo();
								Message.ReplyTo.Email = xmlTextReader.GetAttribute("replyemail");
							}
							if (xmlTextReader.GetAttribute("RECEIPTEMAIL") != null && xmlTextReader.GetAttribute("RECEIPTEMAIL") != string.Empty)
							{
								Message.ReturnReceipt.Email = xmlTextReader.GetAttribute("RECEIPTEMAIL");
							}
							else if (xmlTextReader.GetAttribute("receiptemail") != null && xmlTextReader.GetAttribute("receiptemail") != string.Empty)
							{
								Message.ReturnReceipt.Email = xmlTextReader.GetAttribute("receiptemail");
							}
						}
						switch (xmlTextReader.Name.ToUpper())
						{
						case "FROM":
							Message.From = address;
							break;
						case "TO":
							Message.To.Add(address);
							break;
						case "CC":
							Message.Cc.Add(address);
							break;
						case "BCC":
							Message.Bcc.Add(address);
							break;
						}
						break;
					}
					case "LISTTEMPLATE":
					{
						ListTemplate listTemplate = new ListTemplate();
						string regionID = string.Empty;
						string nullText = string.Empty;
						if (xmlTextReader.GetAttribute("REGIONID") != null && xmlTextReader.GetAttribute("REGIONID") != string.Empty)
						{
							regionID = xmlTextReader.GetAttribute("REGIONID");
						}
						else if (xmlTextReader.GetAttribute("regionid") != null && xmlTextReader.GetAttribute("regionid") != string.Empty)
						{
							regionID = xmlTextReader.GetAttribute("regionid");
						}
						if (xmlTextReader.GetAttribute("NULLTEXT") != null && xmlTextReader.GetAttribute("NULLTEXT") != string.Empty)
						{
							nullText = xmlTextReader.GetAttribute("NULLTEXT");
						}
						else if (xmlTextReader.GetAttribute("nulltext") != null && xmlTextReader.GetAttribute("nulltext") != string.Empty)
						{
							nullText = xmlTextReader.GetAttribute("nulltext");
						}
						if (xmlTextReader.HasAttributes && xmlTextReader.GetAttribute("NAME") != null && xmlTextReader.GetAttribute("NAME") != string.Empty)
						{
							listTemplate = new ListTemplate(xmlTextReader.GetAttribute("NAME"), xmlTextReader.ReadString());
						}
						else if (xmlTextReader.HasAttributes && xmlTextReader.GetAttribute("name") != null && xmlTextReader.GetAttribute("name") != string.Empty)
						{
							listTemplate = new ListTemplate(xmlTextReader.GetAttribute("name"), xmlTextReader.ReadString());
						}
						listTemplate.RegionID = regionID;
						listTemplate.NullText = nullText;
						ListTemplates.Add(listTemplate);
						break;
					}
					case "SMTPSERVER":
					{
						Server server = new Server();
						if (xmlTextReader.GetAttribute("SERVER") != null && xmlTextReader.GetAttribute("SERVER") != string.Empty)
						{
							server.Host = xmlTextReader.GetAttribute("SERVER");
						}
						else if (xmlTextReader.GetAttribute("server") != null && xmlTextReader.GetAttribute("server") != string.Empty)
						{
							server.Host = xmlTextReader.GetAttribute("server");
						}
						if (xmlTextReader.GetAttribute("PORT") != null && xmlTextReader.GetAttribute("PORT") != string.Empty)
						{
							server.Port = int.Parse(xmlTextReader.GetAttribute("PORT"));
						}
						else if (xmlTextReader.GetAttribute("port") != null && xmlTextReader.GetAttribute("port") != string.Empty)
						{
							server.Port = int.Parse(xmlTextReader.GetAttribute("port"));
						}
						if (xmlTextReader.GetAttribute("USERNAME") != null && xmlTextReader.GetAttribute("USERNAME") != string.Empty)
						{
							server.Username = xmlTextReader.GetAttribute("USERNAME");
						}
						else if (xmlTextReader.GetAttribute("username") != null && xmlTextReader.GetAttribute("username") != string.Empty)
						{
							server.Username = xmlTextReader.GetAttribute("username");
						}
						if (xmlTextReader.GetAttribute("PASSWORD") != null && xmlTextReader.GetAttribute("PASSWORD") != string.Empty)
						{
							server.Password = xmlTextReader.GetAttribute("PASSWORD");
						}
						else if (xmlTextReader.GetAttribute("password") != null && xmlTextReader.GetAttribute("password") != string.Empty)
						{
							server.Password = xmlTextReader.GetAttribute("password");
						}
						SmtpServers.Add(server);
						break;
					}
					case "CONDITION":
					{
						Condition condition = new Condition();
						if (xmlTextReader.GetAttribute("REGIONID") != null && xmlTextReader.GetAttribute("REGIONID") != string.Empty)
						{
							condition.RegionID = xmlTextReader.GetAttribute("REGIONID");
						}
						else if (xmlTextReader.GetAttribute("regionid") != null && xmlTextReader.GetAttribute("regionid") != string.Empty)
						{
							condition.RegionID = xmlTextReader.GetAttribute("regionid");
						}
						if (xmlTextReader.GetAttribute("OPERATOR") != null && xmlTextReader.GetAttribute("OPERATOR") != string.Empty)
						{
							condition.Operator = (OperatorType)Enum.Parse(typeof(OperatorType), xmlTextReader.GetAttribute("OPERATOR"), true);
						}
						else if (xmlTextReader.GetAttribute("operator") != null && xmlTextReader.GetAttribute("operator") != string.Empty)
						{
							condition.Operator = (OperatorType)Enum.Parse(typeof(OperatorType), xmlTextReader.GetAttribute("operator"), true);
						}
						if (xmlTextReader.GetAttribute("NULLTEXT") != null && xmlTextReader.GetAttribute("NULLTEXT") != string.Empty)
						{
							condition.NullText = xmlTextReader.GetAttribute("NULLTEXT");
						}
						else if (xmlTextReader.GetAttribute("nulltext") != null && xmlTextReader.GetAttribute("nulltext") != string.Empty)
						{
							condition.NullText = xmlTextReader.GetAttribute("nulltext");
						}
						if (xmlTextReader.GetAttribute("FIELD") != null && xmlTextReader.GetAttribute("FIELD") != string.Empty)
						{
							condition.Field = xmlTextReader.GetAttribute("FIELD");
						}
						else if (xmlTextReader.GetAttribute("field") != null && xmlTextReader.GetAttribute("field") != string.Empty)
						{
							condition.Field = xmlTextReader.GetAttribute("field");
						}
						if (xmlTextReader.GetAttribute("VALUE") != null && xmlTextReader.GetAttribute("VALUE") != string.Empty)
						{
							condition.Value = xmlTextReader.GetAttribute("VALUE");
						}
						else if (xmlTextReader.GetAttribute("value") != null && xmlTextReader.GetAttribute("value") != string.Empty)
						{
							condition.Value = xmlTextReader.GetAttribute("value");
						}
						if (xmlTextReader.GetAttribute("CASESENSITIVE") != null && xmlTextReader.GetAttribute("CASESENSITIVE") != string.Empty)
						{
							condition.CaseSensitive = bool.Parse(xmlTextReader.GetAttribute("CASESENSITIVE"));
						}
						else if (xmlTextReader.GetAttribute("casesensitive") != null && xmlTextReader.GetAttribute("casesensitive") != string.Empty)
						{
							condition.CaseSensitive = bool.Parse(xmlTextReader.GetAttribute("casesensitive"));
						}
						Conditions.Add(condition);
						break;
					}
					case "REGION":
					{
						Region region = new Region();
						if (xmlTextReader.GetAttribute("REGIONID") != null && xmlTextReader.GetAttribute("REGIONID") != string.Empty)
						{
							region.RegionID = xmlTextReader.GetAttribute("REGIONID");
						}
						else if (xmlTextReader.GetAttribute("regionid") != null && xmlTextReader.GetAttribute("regionid") != string.Empty)
						{
							region.RegionID = xmlTextReader.GetAttribute("regionid");
						}
						if (xmlTextReader.GetAttribute("NULLTEXT") != null && xmlTextReader.GetAttribute("NULLTEXT") != string.Empty)
						{
							region.NullText = xmlTextReader.GetAttribute("NULLTEXT");
						}
						else if (xmlTextReader.GetAttribute("nulltext") != null && xmlTextReader.GetAttribute("nulltext") != string.Empty)
						{
							region.NullText = xmlTextReader.GetAttribute("nulltext");
						}
						if (xmlTextReader.GetAttribute("URL") != null && xmlTextReader.GetAttribute("URL") != string.Empty)
						{
							region.URL = xmlTextReader.GetAttribute("URL");
						}
						else if (xmlTextReader.GetAttribute("url") != null && xmlTextReader.GetAttribute("url") != string.Empty)
						{
							region.URL = xmlTextReader.GetAttribute("url");
						}
						Regions.Add(region);
						break;
					}
					}
					break;
				case XmlNodeType.Text:
					switch (text.ToUpper())
					{
					case "SUBJECT":
						Message.Subject += xmlTextReader.Value;
						break;
					case "BODYHTML":
						Message.BodyHtml.Text += xmlTextReader.Value;
						break;
					case "BODYTEXT":
						Message.BodyText.Text += xmlTextReader.Value;
						break;
					}
					break;
				case XmlNodeType.EndElement:
					text = string.Empty;
					break;
				}
			}
		}

		private string LoadFileContent(string filename)
		{
			string empty = string.Empty;
			if (filename.ToUpper().StartsWith("HTTP://") || filename.ToUpper().StartsWith("HTTPS://"))
			{
				empty = new StreamReader(WebRequest.Create(filename).GetResponse().GetResponseStream()).ReadToEnd();
			}
			else
			{
				if (filename.ToUpper().StartsWith("FILE://"))
				{
					filename = filename.Substring(7);
				}
				if (!File.Exists(filename))
				{
					throw new Exception("The specified file doesn't exist.");
				}
				TextReader textReader = TextReader.Synchronized(new StreamReader(filename, Encoding.ASCII));
				empty = textReader.ReadToEnd();
				textReader.Close();
			}
			return empty;
		}

		public void InitReplyTo()
		{
			if (Message.ReplyTo == null)
			{
				Message.ReplyTo = new Address();
			}
		}
	}
}

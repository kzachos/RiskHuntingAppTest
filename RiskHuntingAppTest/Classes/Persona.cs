using System;
using System.Collections.Generic;

namespace RiskHuntingAppTest
{
	public class Persona
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string ImageUrl { get; set; }
		public string ImageRef { get; set; }
		public string ImageSource { get; set; }
		public string Role { get; set; }
		public string Type { get; set; }
		public string Characteristics { get; set; }
		public string Country { get; set; }
		public string DateCreated { get; set; }
		public List<string> Prompts { get; set; }
	}

//	public class NLResponse
//	{
//		public List<NLResponseToken> responseToken { get; set; }
//	}
}


using System;
namespace Hueliday
{
	public class Command
	{
		public string Address { get; set; } //Path to a light resource, a group resource or any other bridge resource (including "/api/<username>/")
		public string Method { get; set; } // GET, PUT, ...
		public object Body { get; set; } //json body of the command

	}
}

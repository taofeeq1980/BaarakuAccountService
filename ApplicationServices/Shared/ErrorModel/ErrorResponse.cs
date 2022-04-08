using System.Collections.Generic;

namespace ApplicationServices.Shared.ErrorModel
{
	public class ErrorResponse
	{
		public List<Error> Errors { get; set; } = new List<Error>();
	}
}
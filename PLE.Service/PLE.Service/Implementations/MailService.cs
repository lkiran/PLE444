using System;
using System.Net.Mail;
using System.Threading.Tasks;
using PLE.Contract.DTOs.Requests;
using PLE.Service.App_Start;
using PLE.Service.Helpers;

namespace PLE.Service.Implementations
{
	public class MailService
	{
		#region Fields
		private readonly EmailService _ms;
		#endregion

		#region Ctor
		public MailService() {
			_ms = new EmailService();
		} 
		#endregion

		public async Task<bool> SendMail(MailDto request) {
			try
			{
				var rendered = ViewRenderer.RenderView($"~/Views/MailTemplates/{request.Template}.cshtml", request.Model);
				var mail = new MailMessage {
					Subject = request.Title,
					Body = rendered,
					IsBodyHtml = true,
					
				};

				foreach (var receiver in request.To)
					mail.To.Add(receiver);
				foreach (var receiver in request.Bcc)
					mail.Bcc.Add(receiver);
				
				await _ms.SendAsync(mail);
			}
			catch(Exception ex) {
				Console.WriteLine(ex.ToString());
				return false;
			}

			return true;
		}
	}
}
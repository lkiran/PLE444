using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PLE444.Models
{
	public class PleClaimsIdentity : ClaimsIdentity
	{
		public PleClaimsIdentity(IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType) { }

		public override void AddClaim(Claim claim) {
			var existingClaim = Claims.FirstOrDefault(c => c.Value == claim.Value && c.Type == claim.Type);
			if (existingClaim != null)
				return;
			base.AddClaim(claim);
		}

		public override void AddClaims(IEnumerable<Claim> claims) {
			foreach (var claim in claims)
				AddClaim(claim);
		}
	}
}
using System;
using Backend.Models;

namespace Backend.ViewModels.User
{
	public class UserSubscriptionVM
	{
		public AppUser User { get; set; }
		public Pricing Pricing { get; set; }
	}
}


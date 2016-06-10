using System;
using System.Collections.Generic;
using EventSourcedMerging.Events;
using Ledger;

namespace EventSourcedMerging.Domain
{
	public class User : AggregateRoot<int>
	{
		public string Name { get; private set; }
		public DateTime DateOfBirth { get; private set; }
		public Dictionary<PhoneType, string> Phones { get; private set; }

		private User()
		{
			Phones = new Dictionary<PhoneType, string>();
		}

		public static User Create(int id, string name)
		{
			var user = new User();
			user.ApplyEvent(new UserCreatedEvent(id, name));

			return user;
		}

		public void PushEvent(DomainEvent<int> domainEvent)
		{
			ApplyEvent(domainEvent);
		}

		private void Handle(UserCreatedEvent e)
		{
			ID = e.ID;
			Name = e.Name;
		}

		private void Handle(NameMergedEvent e)
		{
			Name = e.NewName;
		}

		private void Handle(DateOfBirthChangedEvent e)
		{
			DateOfBirth = e.DateOfBirth;
		}

		private void Handle(HomePhoneChangedEvent e)
		{
			Phones[PhoneType.Home] = e.HomeNumber;
		}

		private void Handle(MobilePhoneMergedEvent e)
		{
			Phones[PhoneType.Mobile] = e.MobileNumber;
		}

		private void Handle(MobilePhoneChangedEvent e)
		{
			Phones[PhoneType.Mobile] = e.MobileNumber;
		}
	}

	namespace Events
	{
	}
}

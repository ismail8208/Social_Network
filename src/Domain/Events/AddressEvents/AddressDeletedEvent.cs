namespace MediaLink.Domain.Events.AddressEvents;

public class AddressDeletedEvent : BaseEvent
{
    public AddressDeletedEvent(Address address) 
    {
        Address = address;
    }

public Address Address { get; }
}


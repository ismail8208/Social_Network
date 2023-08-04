namespace MediaLink.Domain.Events.AddressEvents;

public class AddressCreatedEvent : BaseEvent
{
    public AddressCreatedEvent(Address address)
    {
        Address = address;
    }

    public Address Address { get; }
}

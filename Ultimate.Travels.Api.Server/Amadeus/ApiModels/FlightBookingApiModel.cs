namespace Ultimate.Travels.Api.Server
{
    public class FlightBookingApiModel
    {
        public FlightBookingData Data { get; set; }

        public class FlightBookingData
        {
            public string Type => "flight-order";
            public List<FlightDataApiModel> FlightOffers { get; set; }
            public List<TravelerApiModel> Travelers { get; set; }
            public Remarks Remarks { get; set; }
            public TicketingAgreement TicketingAgreement { get; set; }
            public List<ContactInfo> Contacts { get; set; }
        }

        public class Names
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Phone
        {
            public string DeviceType { get; set; }
            public string CountryCallingCode { get; set; }
            public string Number { get; set; }
        }

        public class GeneralRemark
        {
            public string SubType { get; set; }
            public string Text { get; set; }
        }

        public class Remarks
        {
            public List<GeneralRemark> General { get; set; }
        }

        public class TicketingAgreement
        {
            public string Option { get; set; }
            public string Delay { get; set; }
        }

        public class Address
        {
            public List<string> Lines { get; set; }
            public string PostalCode { get; set; }
            public string CityName { get; set; }
            public string CountryCode { get; set; }
        }

        public class ContactInfo
        {
            public Names AddresseeName { get; set; }
            public string CompanyName { get; set; }
            public string Purpose { get; set; }
            public List<Phone> Phones { get; set; }
            public string EmailAddress { get; set; }
            public Address Address { get; set; }
        }

        public class TravelerContact
        {
            public string EmailAddress { get; set; }
            public List<Phone> Phones { get; set; }
        }

        public class TravelerDocument
        {
            public string DocumentType { get; set; }
            public string BirthPlace { get; set; }
            public string IssuanceLocation { get; set; }
            public string IssuanceDate { get; set; }
            public string Number { get; set; }
            public string ExpiryDate { get; set; }
            public string IssuanceCountry { get; set; }
            public string ValidityCountry { get; set; }
            public string Nationality { get; set; }
            public bool Holder { get; set; }
        }

        public class TravelerApiModel
        {
            public string Id { get; set; }
            public string DateOfBirth { get; set; }
            public Names Name { get; set; }
            public string Gender { get; set; }
            public TravelerContact Contact { get; set; }
            public List<TravelerDocument> Documents { get; set; }
        }
    }
}

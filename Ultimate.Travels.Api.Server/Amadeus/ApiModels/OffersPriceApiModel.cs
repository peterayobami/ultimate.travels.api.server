namespace Ultimate.Travels.Api.Server
{
    public class OffersPriceApiModel
    {
        public OffersPriceData Data { get; set; }
        public DictionariesApiModel Dictionaries { get; set; }
    }

    public class OffersPriceData
    {
        public string Type => "flight-offers-pricing";
        public List<FlightDataApiModel> FlightOffers { get; set; }
        public BookingRequirements BookingRequirements { get; set; }
    }

    public class BookingRequirements
    {
        public bool EmailAddressRequired { get; set; }
        public bool MobilePhoneNumberRequired { get; set; }
    }
}
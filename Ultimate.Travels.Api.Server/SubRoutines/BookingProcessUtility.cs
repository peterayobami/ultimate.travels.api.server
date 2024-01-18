namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Utility methods for processing booking
    /// </summary>
    public static class BookingProcessUtility
    {
        /// <summary>
        /// Compose the credentials for flight booking
        /// </summary>
        /// <param name="offer">The specified fligh offer</param>
        /// <param name="flightBooking">The flight booking details</param>
        /// <returns></returns>
        public static FlightBookingApiModel ComposeCredentialsAsync(FlightDataApiModel offer, FlightBookingDataModel flightBooking)
        {
            // Initialize travelers
            List<FlightBookingApiModel.TravelerApiModel> travelers = [];

            // For each traveler...
            foreach (var traveler in flightBooking.Travelers)
            {
                // Create and add traveler to the collection
                travelers.Add(new FlightBookingApiModel.TravelerApiModel
                {
                    Id = traveler.TravelerId,
                    DateOfBirth = traveler.DateOfBirth,
                    Name = new FlightBookingApiModel.Names
                    {
                        FirstName = traveler.FirstName,
                        LastName = traveler.LastName
                    },
                    Gender = traveler.Gender.ToUpper(),
                    Contact = new FlightBookingApiModel.TravelerContact
                    {
                        EmailAddress = flightBooking.Customer.Email,
                        Phones = [
                            new FlightBookingApiModel.Phone
                            {
                                CountryCallingCode = flightBooking.Customer.CountryDialingCode,
                                DeviceType = "MOBILE",
                                Number = flightBooking.Customer.Phone,
                            }
                        ]
                    }
                });
            }

            // Return the result
            return new FlightBookingApiModel
            {
                Data = new FlightBookingApiModel.FlightBookingData
                {
                    FlightOffers = [offer],
                    Travelers = travelers,
                    Remarks = new FlightBookingApiModel.Remarks
                    {
                        General =
                        [
                            new FlightBookingApiModel.GeneralRemark
                            {
                                SubType = "GENERAL_MISCELLANEOUS",
                                Text = "ONLINE BOOKING FROM ULTIMATE TRAVELS"
                            }
                        ]
                    },
                    TicketingAgreement = new FlightBookingApiModel.TicketingAgreement
                    {
                        Option = "DELAY_TO_CANCEL",
                        Delay = "1D"
                    },
                    Contacts =
                    [
                        new FlightBookingApiModel.ContactInfo
                        {
                            AddresseeName = new FlightBookingApiModel.Names
                            {
                                FirstName = "PETE",
                                LastName = "MOSS"
                            },
                            CompanyName = "ULTIMATE TRAVELS",
                            Purpose = "STANDARD",
                            Phones =
                            [
                                new FlightBookingApiModel.Phone
                                {
                                    CountryCallingCode = "234",
                                    DeviceType = "MOBILE",
                                    Number = "9029223540"
                                }
                            ],
                            EmailAddress = "info@ultimatetravels.com",
                            Address = new FlightBookingApiModel.Address
                            {
                                Lines = ["67, College Rd"],
                                PostalCode = "100218",
                                CityName = "LAGOS",
                                CountryCode = "NG"
                            }
                        }
                    ]
                }
            };
        }
    }
}

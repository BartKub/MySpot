namespace MySpot.Api.Exceptions
{
    public class InvalidLicencePlateException: CustomException
    {
        public string LicencePlate { get; }

        public InvalidLicencePlateException(string licencePlate) : base($"Licence plate: {licencePlate} is invalid.")
        {
            LicencePlate = licencePlate;
        }
    }
}

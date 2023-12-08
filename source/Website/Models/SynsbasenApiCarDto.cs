namespace Website.Models
{
    public class SynsbasenApiCarDto
    {
        public Data data { get; set; }
        public int cost { get; set; }
    }

    public class Data
    {
        public double id { get; set; }
        public string registration { get; set; }
        public string vin { get; set; }
        public string first_registration_date { get; set; }
        public string status { get; set; }
        public string status_updated_at { get; set; }
        public string registration_status { get; set; }
        public string registration_status_updated_at { get; set; }
        public string kind { get; set; }
        public string usage { get; set; }
        public string category { get; set; }
        public int model_year { get; set; }
        public string fuel_type { get; set; }
        public double mileage { get; set; }
        public double mileage_annual_average { get; set; }
        public string brand { get; set; }
        public string brand_id { get; set; }
        public string model { get; set; }
        public string model_id { get; set; }
        public string variant { get; set; }
        public string variant_id { get; set; }
        public string version { get; set; }
        public string version_id { get; set; }
        public string body_type { get; set; }
        public string eu_version { get; set; }
        public string eu_variant { get; set; }
        public string ec_type_approval { get; set; }
        public string last_inspection_date { get; set; }
        public string last_inspection_result { get; set; }
        public string last_inspection_kind { get; set; }
        public bool ncap_five { get; set; }
        public string leasing_period_start { get; set; }
        public string leasing_period_end { get; set; }
        public string extra_equipment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.Recognition.FPT
{
    public class OCRResponse
    {
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }

    public class OCRAddressEntities
    {
        [JsonPropertyName("province")]
        public string Province { get; set; }
        [JsonPropertyName("district")]
        public string District { get; set; }
        [JsonPropertyName("ward")]
        public string Ward { get; set; }
        [JsonPropertyName("street")]
        public string Street { get; set; }
    }

    public class OCRFrontIdDataNewType
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("id_prob")]
        public string IdProb { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("name_prob")]
        public string NameProb { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("dob_prob")]
        public string DobProb { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("sex_prob")]
        public string SexProb { get; set; }

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("nationality_prob")]
        public string NationalityProb { get; set; }

        [JsonPropertyName("home")]
        public string Home { get; set; }

        [JsonPropertyName("home_prob")]
        public string HomeProb { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("address_prob")]
        public string AddressProb { get; set; }

        [JsonPropertyName("doe")]
        public string Doe { get; set; }

        [JsonPropertyName("doe_prob")]
        public string DoeProb { get; set; }

        [JsonPropertyName("overall_score")]
        public string OverallScore { get; set; }

        [JsonPropertyName("address_entities")]
        public OCRAddressEntities AddressEntities { get; set; }

        [JsonPropertyName("type_new")]
        public string TypeNew { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class OCRBackIdDataOldType
    {
        [JsonPropertyName("religion")]
        public string Religion { get; set; }

        [JsonPropertyName("religion_prob")]
        public string ReligionProb { get; set; }

        [JsonPropertyName("ethnicity")]
        public string Ethnicity { get; set; }

        [JsonPropertyName("ethnicity_prob")]
        public string EthnicityProb { get; set; }

        [JsonPropertyName("features")]
        public string Features { get; set; }

        [JsonPropertyName("features_prob")]
        public string FeaturesProb { get; set; }

        [JsonPropertyName("issue_date")]
        public string IssueDate { get; set; }

        [JsonPropertyName("issue_date_prob")]
        public string IssueDateProb { get; set; }

        [JsonPropertyName("issue_loc_prob")]
        public string IssueLocProb { get; set; }

        [JsonPropertyName("issue_loc")]
        public string IssueLoc { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class OCRBackIdDataNewType
    {
        [JsonPropertyName("religion")]
        public string Religion { get; set; }

        [JsonPropertyName("religion_prob")]
        public string ReligionProb { get; set; }

        [JsonPropertyName("ethnicity")]
        public string Ethnicity { get; set; }

        [JsonPropertyName("ethnicity_prob")]
        public string EthnicityProb { get; set; }

        [JsonPropertyName("features")]
        public string Features { get; set; }

        [JsonPropertyName("features_prob")]
        public string FeaturesProb { get; set; }

        [JsonPropertyName("issue_date")]
        public string IssueDate { get; set; }

        [JsonPropertyName("issue_date_prob")]
        public string IssueDateProb { get; set; }

        [JsonPropertyName("issue_loc")]
        public string IssueLoc { get; set; }

        [JsonPropertyName("issue_loc_prob")]
        public string IssueLocProb { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class OCRDataPassport
    {
        [JsonPropertyName("passport_number")]
        public string PassportNumber { get; set; }

        [JsonPropertyName("passport_number_prob")]
        public string PassportNumberProb { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("name_prob")]
        public string NameProb { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("dob_prob")]
        public string DobProb { get; set; }

        [JsonPropertyName("pob")]
        public string Pob { get; set; }

        [JsonPropertyName("pob_prob")]
        public string PobProb { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("sex_prob")]
        public string SexProb { get; set; }

        [JsonPropertyName("id_number")]
        public string IdNumber { get; set; }

        [JsonPropertyName("id_number_prob")]
        public string IdNumberProb { get; set; }

        [JsonPropertyName("doi")]
        public string Doi { get; set; }

        [JsonPropertyName("doi_prob")]
        public string DoiProb { get; set; }

        [JsonPropertyName("doe")]
        public string Doe { get; set; }

        [JsonPropertyName("doe_prob")]
        public string DoeProb { get; set; }

        [JsonPropertyName("poi")]
        public string IdIssuer { get; set; }

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }
    }

    /// <summary>
    /// Response for front Id card new type
    /// </summary>
    public class OCRResponseFrontIdNewType : OCRResponse
    {
        [JsonPropertyName("data")]
        public List<OCRFrontIdDataNewType> Data { get; set; }
    }

    /// <summary>
    /// Response for back Id card new type
    /// </summary>
    public class OCRResponseBackIdNewType : OCRResponse
    {
        [JsonPropertyName("data")]
        public List<OCRBackIdDataNewType> Data { get; set; }
    }

    /// <summary>
    /// Response for passport new type
    /// </summary>
    public class OCRResponsePassport : OCRResponse
    {
        [JsonPropertyName("data")]
        public List<OCRDataPassport> Data { get; set; }
    }
}
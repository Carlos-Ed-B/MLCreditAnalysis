using CreditAnalysis.Model.Enums;
using Infrastructure.Layer.Helpers.Json;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;

namespace CreditAnalysis.Model
{
    public class ClientCreditAnalysisModel
    {
        public string Id { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Salário")]
        public float Salary { get; set; }

        [Display(Name = "Outras Rendas")]
        public bool ExtraSalary { get; set; }

        [Display(Name = "Etnia")]
        public int Ethnicity { get; set; }

        [Display(Name = "Estado Civil")]
        public int MaritalStatus { get; set; }

        [Display(Name = "Escolaridade")]
        public int Schooling { get; set; }

        [Display(Name = "Casa Propria")]
        public bool OwnHome { get; set; }

        [Display(Name = "Idade")]
        public int Age { get; set; }

        [Display(Name = "Genero")]
        public GenderEnum Gender { get; set; }

        [JsonIgnore]
        public byte[] ImageFile { get; set; }
        public string ImageFileBase64 { get; set; }
        public string ImagePath { get; set; }
        [JsonIgnore]
        public IFormFile FileUpload { get; set; }
        [JsonIgnore]
        public byte[] FileUploadByte { get; set; }

        public string MessageError { get; set; }
        public long VisionFaceAge { get; set; }
        public string VisionFaceGender { get; set; }
    }
}

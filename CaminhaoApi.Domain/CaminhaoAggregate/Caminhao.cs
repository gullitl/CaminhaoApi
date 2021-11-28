using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaminhaoApi.Domain.CaminhaoAggregate
{
    public class Caminhao : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Marca { get; set; }
        [Required]
        public Modelo Modelo { get; set; }
        [Required]
        public int AnoFabricacao { get; set; }
        [Required]
        public int AnoModelo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Modelo != Modelo.FH && Modelo != Modelo.FM)
                yield return new ValidationResult("Modelo pode aceitar apenas FH e FM");

            if (AnoFabricacao != DateTime.Now.Year)
                yield return new ValidationResult("Ano de Fabricação deve ser o atual");

            if (AnoModelo != DateTime.Now.Year && AnoModelo != DateTime.Now.Year + 1)
                yield return new ValidationResult("Ano de Modelo pode ser o atual ou o ano subsequente");
        }

        public override bool Equals(object obj) =>
            obj is Caminhao caminhao &&
            Id == caminhao.Id &&
            Marca == caminhao.Marca &&
            Modelo == caminhao.Modelo &&
            AnoFabricacao == caminhao.AnoFabricacao &&
            AnoModelo == caminhao.AnoModelo;

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(Id);
            hash.Add(Marca);
            hash.Add(Modelo);
            hash.Add(AnoFabricacao);
            hash.Add(AnoModelo);
            return hash.ToHashCode();
        }


    }
}

using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class AddressSeeder
{
    private readonly ApplicationDbContext _context;

    public AddressSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Addresses.Any())
        {
            var addresses = new List<Address>
            {
                new Address {
                    Logradouro = "Avenida Paulista",
                    Cep = "01311-000",
                    Bairro = "Bela Vista",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Pais = "Brasil",
                    Numero = "1578",
                    Complemento = "Apartamento 101"
                },
                new Address {
                    Logradouro = "Rua das Flores",
                    Cep = "11026-033",
                    Bairro = "Centro",
                    Cidade = "Curitiba",
                    Estado = "PR",
                    Pais = "Brasil",
                    Numero = "254",
                    Complemento = "Casa"
                },
                new Address {
                    Logradouro = "Avenida Atlântica",
                    Cep = "22070-000",
                    Bairro = "Copacabana",
                    Cidade = "Rio de Janeiro",
                    Estado = "RJ",
                    Pais = "Brasil",
                    Numero = "3200",
                    Complemento = "Cobertura"
                },
                new Address {
                    Logradouro = "Rua das Palmeiras",
                    Cep = "30140-120",
                    Bairro = "Savassi",
                    Cidade = "Belo Horizonte",
                    Estado = "MG",
                    Pais = "Brasil",
                    Numero = "45",
                    Complemento = "Sala 305"
                },
                new Address {
                    Logradouro = "Rua Augusta",
                    Cep = "01413-000",
                    Bairro = "Consolação",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Pais = "Brasil",
                    Numero = "650",
                    Complemento = "Fundos"
                },
                new Address {
                    Logradouro = "Rua do Sol",
                    Cep = "40050-030",
                    Bairro = "Pelourinho",
                    Cidade = "Salvador",
                    Estado = "BA",
                    Pais = "Brasil",
                    Numero = "120",
                    Complemento = "Prédio Azul"
                },
                new Address {
                    Logradouro = "Avenida Beira-Mar",
                    Cep = "60165-121",
                    Bairro = "Meireles",
                    Cidade = "Fortaleza",
                    Estado = "CE",
                    Pais = "Brasil",
                    Numero = "908",
                    Complemento = "Edifício Vista Mar"
                },
                new Address {
                    Logradouro = "Rua das Acácias",
                    Cep = "88034-100",
                    Bairro = "Lagoa da Conceição",
                    Cidade = "Florianópolis",
                    Estado = "SC",
                    Pais = "Brasil",
                    Numero = "333",
                    Complemento = "Chalé 7"
                },
                new Address {
                    Logradouro = "Avenida Goiás",
                    Cep = "74063-010",
                    Bairro = "Setor Central",
                    Cidade = "Goiânia",
                    Estado = "GO",
                    Pais = "Brasil",
                    Numero = "500",
                    Complemento = "Bloco B"
                },
                new Address {
                    Logradouro = "Rua do Comércio",
                    Cep = "50010-030",
                    Bairro = "Recife Antigo",
                    Cidade = "Recife",
                    Estado = "PE",
                    Pais = "Brasil",
                    Numero = "89",
                    Complemento = "Sala 12"
                }
            };

            _context.Addresses.AddRange(addresses);
            _context.SaveChanges();
        }
    }
}

using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class MedicalExamSeeder
{
    private readonly ApplicationDbContext _context;

    public MedicalExamSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.MedicalExams.Any())
        {
            var MedicalExams = new List<MedicalExam>
            {
                new MedicalExam {
                    Name = "Hemograma Completo",
                    Description = "Exame de sangue que avalia as células sanguíneas, como glóbulos vermelhos, brancos e plaquetas, para verificar a saúde geral e detectar condições como anemia, infecções e outras doenças."
                },
                new MedicalExam {
                    Name = "Glicemia em Jejum",
                    Description = "Exame que mede o nível de glicose no sangue após um período de jejum, utilizado para diagnosticar diabetes ou monitorar o controle glicêmico."
                },
                new MedicalExam {
                    Name = "Colesterol Total e Frações",
                    Description = "Avalia os níveis de colesterol no sangue, incluindo HDL (bom colesterol), LDL (mau colesterol) e triglicerídeos, para verificar o risco de doenças cardiovasculares."
                },
                new MedicalExam {
                    Name = "Exame de Urina (EAS)",
                    Description = "Analisa a urina para detectar problemas renais, infecções urinárias, diabetes e outras condições, avaliando aspectos como cor, pH, presença de proteínas e células."
                },
                new MedicalExam {
                    Name = "Ressonância Magnética",
                    Description = "Exame de imagem que utiliza campos magnéticos e ondas de rádio para obter imagens detalhadas de órgãos e tecidos, auxiliando no diagnóstico de lesões, tumores e doenças neurológicas."
                },
                new MedicalExam {
                    Name = "Ultrassonografia Abdominal",
                    Description = "Exame de imagem que utiliza ondas sonoras para visualizar órgãos abdominais, como fígado, rins, pâncreas e baço, ajudando a identificar alterações ou doenças."
                },
                new MedicalExam {
                    Name = "Teste de Função Tireoidiana (T3, T4, TSH)",
                    Description = "Avalia o funcionamento da tireoide, medindo os níveis dos hormônios T3, T4 e TSH, para diagnosticar hipotireoidismo, hipertireoidismo e outras disfunções tireoidianas."
                },
                new MedicalExam {
                    Name = "Eletrocardiograma (ECG)",
                    Description = "Registra a atividade elétrica do coração, ajudando a identificar arritmias, infartos, problemas cardíacos e outras condições relacionadas ao sistema cardiovascular."
                },
                new MedicalExam {
                    Name = "Tomografia Computadorizada",
                    Description = "Exame de imagem que utiliza raios-X para criar imagens detalhadas de estruturas internas do corpo, como ossos, órgãos e vasos sanguíneos, auxiliando no diagnóstico de tumores, fraturas e outras doenças."
                },
                new MedicalExam {
                    Name = "Exame de Fezes (Parasitológico)",
                    Description = "Analisa as fezes para detectar a presença de parasitas, bactérias ou outros microrganismos que possam causar infecções gastrointestinais."
                }
            };


            _context.MedicalExams.AddRange(MedicalExams);
            _context.SaveChanges();
        }
    }
}

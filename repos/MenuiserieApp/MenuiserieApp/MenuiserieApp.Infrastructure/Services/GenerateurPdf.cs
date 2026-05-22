using MenuiserieApp.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;

namespace MenuiserieApp.Services
{
    public class GenerateurPdf
    {
        public static void CreerFacture(Commande commande)
        {
            var cheminFichier = $"Facture_{commande.NumeroReference}.pdf";

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Element(entete =>
                    {
                        entete.Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Menuiserie PVC").FontSize(20).SemiBold().FontColor(Colors.Blue.Darken2);
                                col.Item().Text("123 Rue de l Atelier");
                                col.Item().Text("76700 Harfleur");
                            });
                            row.ConstantItem(100).Height(50).Placeholder();
                        });
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().Text($"Facture {commande.NumeroReference}").FontSize(16).Bold();
                        col.Item().Text($"Client : {commande.Client?.Nom}");
                        col.Item().Text($"Date : {commande.DateCommande:dd/MM/yyyy}");

                        col.Item().PaddingTop(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Photo").SemiBold();
                                header.Cell().Text("Produit").SemiBold();
                                header.Cell().Text("H").SemiBold();
                                header.Cell().Text("L").SemiBold();
                                header.Cell().Text("Qté").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });
                            decimal total = 0;
                            foreach (var ligne in commande.LigneCommandes)
                            {
                                if (!string.IsNullOrEmpty(ligne.CheminImage) && System.IO.File.Exists(ligne.CheminImage))
                                {
                                    table.Cell().MaxWidth(100).MaxHeight(100).Image(ligne.CheminImage);
                                }
                                else
                                {
                                    table.Cell().AlignCenter().AlignMiddle().Text("-");
                                }
                                table.Cell().Text(ligne.Designation);
                                table.Cell().Text(ligne.HauteurMm.ToString());
                                table.Cell().Text(ligne.LargeurMm.ToString());
                                table.Cell().Text(ligne.Quantite.ToString());
                                table.Cell().Text($"{ligne.TotalLigne} euros");

                                total += ligne.TotalLigne;
                            }

                            table.Cell().ColumnSpan(5).AlignRight().PaddingRight(10).Text("Total Général :").SemiBold();

                            table.Cell().Text($"{total} euros").SemiBold().FontColor(Colors.Blue.Darken2);
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
            })
            .GeneratePdf(cheminFichier);
            Process.Start(new ProcessStartInfo(cheminFichier) { UseShellExecute = true });
        }
    }
}
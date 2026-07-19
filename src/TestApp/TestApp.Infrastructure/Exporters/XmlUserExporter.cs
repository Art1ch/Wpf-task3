using System.Xml;
using TestApp.Application.Abstractions;
using TestApp.Core.Models;

namespace TestApp.Infrastructure.Exporters;

internal sealed class XmlUserExporter : IUserExporter
{
    public string Name { get; } = "Xml exporter";
    public string FileExtension { get; } = ".xml";

    public void Export(IEnumerable<UserExportModel> users, string folderPath)
    {
        if (users == null || !users.Any())
            throw new ArgumentException("No data for export");

        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            Encoding = System.Text.Encoding.UTF8,
            OmitXmlDeclaration = false
        };

        Directory.CreateDirectory(folderPath);

        var fileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}{FileExtension}";

        var filePath = Path.Combine(folderPath, fileName);

        using var writer = XmlWriter.Create(filePath, settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("Users");

        foreach (var user in users)
        {
            writer.WriteStartElement("Record");

            writer.WriteAttributeString("id", user.Id.ToString());

            writer.WriteElementString("DataCollectedDate", user.DataCollectedDate.ToShortDateString() ?? string.Empty);
            writer.WriteElementString("FirstName", user.FirstName ?? string.Empty);
            writer.WriteElementString("LastName", user.LastName ?? string.Empty);
            writer.WriteElementString("MiddleName", user.MiddleName ?? string.Empty);
            writer.WriteElementString("City", user.City ?? string.Empty);
            writer.WriteElementString("Country", user.Country ?? string.Empty);

            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
    }
}
using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Adobe.PDFServicesSDK;
using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.exception;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.options.exportpdf;
using Adobe.PDFServicesSDK.pdfops;
using PDF_Converter.UI;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        UI ui = new UI();
        string directoryPath;
        try
        {
            // Initial setup, create credentials instance.
            Credentials credentials = Credentials.ServicePrincipalCredentialsBuilder()
                .WithClientId("677957e147534202b16e70cc29c62062")
                .WithClientSecret("p8e-s8a4k7fT3Cxbp9i4qaKF-8r9L5agdHCP")
                .Build();

            var options = new ParallelOptions { MaxDegreeOfParallelism = 2 }; // Limits the number of concurrent operations.


            string[] files = ui.GetFiles();
            ExecutionContext executionContext = ExecutionContext.Create(credentials);
            Parallel.ForEach(files, options, file =>
            {
                Console.WriteLine(file);
                //Create an ExecutionContext using credentials and create a new operation instance.
                ExportPDFOperation exportPdfOperation = ExportPDFOperation.CreateNew(ExportPDFTargetFormat.DOCX);

                // Set operation input from a local PDF file
                FileRef sourceFileRef = FileRef.CreateFromLocalFile(file);
                exportPdfOperation.SetInput(sourceFileRef);

                // Execute the operation.
                FileRef result = exportPdfOperation.Execute(executionContext);

                // Save the result to the specified location.


                directoryPath = Path.Combine(ui.GetDirectoryPath(), "Output");
                Directory.CreateDirectory(directoryPath);

                string fileName = Path.GetFileNameWithoutExtension(file) + ".docx";
                string outputPath = Path.Combine(directoryPath, fileName);
                if (File.Exists(outputPath))
                {
                    string newFileName = Path.GetFileNameWithoutExtension(file) + "_" + Guid.NewGuid().ToString() + ".docx";
                    outputPath = Path.Combine(directoryPath, newFileName);
                }
                result.SaveAs(outputPath);
                Console.WriteLine($"File {file} has been saved to: {outputPath}");
            });

            Process.Start(Path.Combine(ui.GetDirectoryPath(), "Output"));
        }
        
        catch (Adobe.PDFServicesSDK.exception.ServiceUsageException ex)
        {
            Console.Write("API call limit reached", ex);
        }
        catch (Exception ex)
        {
            Console.Write("Exception encountered while executing operation", ex);
        }
    }

   
}

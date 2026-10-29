<%@ WebHandler Language="C#" Class="GetImage" %>

using System;
using System.IO;
using System.Web;

public class GetImage : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        // Retrieves the file path passed from the table component
        string filePath = context.Request.QueryString["path"];

        if (!string.IsNullOrEmpty(filePath))
        {
            // Check if the file exists on the network share/server
            if (File.Exists(filePath))
            {
                // Set content type and stream the file directly to the browser
                context.Response.ContentType = "image/jpeg";
                context.Response.TransmitFile(filePath);
                return;
            }
        }

        // If file not found or path is empty, return 404
        context.Response.StatusCode = 404;
        context.Response.End();
    }

    public bool IsReusable => false;
}
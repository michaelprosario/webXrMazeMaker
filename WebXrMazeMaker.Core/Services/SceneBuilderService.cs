using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WebXrMazeMaker.Core.Services
{
    public class MakeSceneFromImageCommand
    {
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
    }

    public class AppResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class Point2d
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
    }

    public class MakeSceneFromImageResponse : AppResponse
    {
        public MakeSceneFromImageResponse()
        {
            Points = new List<Point2d>();
        }

        public List<Point2d> Points { get; set; }
    }

    public class PictureData
    {
        public PictureData(int width, int height)
        {
            Width = width;
            Height = height;

            Grid = new int[width, height];
        }

        public int[,] Grid { get; set; }
        public int Width { get; }

        public int Height { get; }
    }

    public interface IImageFunctions
    {
        PictureData GetPixelDataForImage(MakeSceneFromImageCommand command);
    }

    public class SceneBuilderService
    {
        private readonly IImageFunctions _imageFunctions;

        public SceneBuilderService(IImageFunctions imageFunctions)
        {
            _imageFunctions = imageFunctions ?? throw new ArgumentNullException(nameof(imageFunctions));
        }

        public MakeSceneFromImageResponse MakeSceneFromImage(MakeSceneFromImageCommand command)
        {
            var response = new MakeSceneFromImageResponse();
            var pictureData = _imageFunctions.GetPixelDataForImage(command);


            var points = new List<Point2d>();
            for (var x = 0; x < pictureData.Width; x++)
            for (var y = 0; y < pictureData.Height; y++)
            {
                var pixelValue = pictureData.Grid[x, y] / 20;
                if (pixelValue > 0)
                    points.Add(new Point2d
                    {
                        Height = pixelValue,
                        X = x,
                        Y = y
                    });
            }

            response.Points = points;

            var jsonString = JsonConvert.SerializeObject(response.Points);
            File.WriteAllText(command.OutputPath, jsonString);

            return response;
        }
    }
}
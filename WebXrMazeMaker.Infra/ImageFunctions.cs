using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using WebXrMazeMaker.Core.Services;

namespace WebXrMazeMaker.Infra
{
    public class ImageFunctions : IImageFunctions
    {
        public PictureData GetPixelDataForImage(MakeSceneFromImageCommand command)
        {
            // load bitmap from file
            // find the width and height of image 
            // load pixel data from bit map into 2d array 
            int width;
            int height;

            int[,] pixelGrid;
            using (var image = Image.Load<Rgba32>(command.InputPath))
            {
                width = image.Width;
                height = image.Height;
                pixelGrid = new int[width, height];
                for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    var color = image[x, y];
                    pixelGrid[x, y] = color.R;
                }
            }

            var pictureData = new PictureData(width, height)
            {
                Grid = pixelGrid
            };
            return pictureData;
        }
    }
}
using NUnit.Framework;
using WebXrMazeMaker.Core.Services;
using WebXrMazeMaker.Infra;

namespace WebXrMazeMaker.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SceneBuilderService__MakeSceneFromImage__ProcessValidInputs()
        {
            // arrange
            IImageFunctions imageFunctions = new ImageFunctions();
            var service = new SceneBuilderService(imageFunctions);

            var command = new MakeSceneFromImageCommand
            {
                InputPath = "C:\\dev\\WebXrMazeMaker\\webXrMazeMaker\\data\\input.jpg",
                OutputPath = "C:\\dev\\WebXrMazeMaker\\webXrMazeMaker\\data\\output.json"
            };

            // act
            var response = service.MakeSceneFromImage(command);

            // assert

            /* Sample output
[
{ X: 1, Y: 1, Height: 20 },
{ X: 2, Y: 2, Height: 20 },
{ X: 3, Y: 3, Height: 20 }
]
             */
            Assert.NotNull(response);
            Assert.IsTrue(response.Points.Count > 0);
            FileAssert.Exists(command.OutputPath);
        }
    }
}
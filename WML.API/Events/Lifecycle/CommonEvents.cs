using Microsoft.Xna.Framework.Graphics;
using OneShotMG;

namespace WorldMachineLoader.API.Events.Lifecycle
{
    /// <summary>Event that is triggered when the graphics device gets initialized.</summary>
    public class GraphicsDeviceInit
    {
        /// <summary>Graphics Device Instance.</summary>
        public GraphicsDevice GraphicsDevice;

        /// <summary>Initializes a new instance of the <see cref="GraphicsDeviceInit"/> class.</summary>
        /// <param name="gDevice">Graphics Device.</param>
        public GraphicsDeviceInit(GraphicsDevice gDevice)
        {
            GraphicsDevice = gDevice;
        }
    }

    /// <summary>Event that is triggered when game starts to initialize.</summary>
    public class Game1InitializeEvent
    {
        public Game1 Instance;

        public Game1InitializeEvent(Game1 instance)
        {
            Instance = instance;
        }
    }

    /// <summary>Event that is triggered when the save file gets written.</summary>
    public class SaveWrittenEvent { }
}

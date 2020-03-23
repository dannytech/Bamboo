using System.Collections.Generic;

namespace Bamboo.Game.Chat
{
    class Key
    {
        public static string Attack { get; } = "key.key.attack";
        public static string Use { get; } = "key.use";
        public static string Forward { get; } = "key.forward";
        public static string Back { get; } = "key.back";
        public static string Left { get; } = "key.left";
        public static string Right { get; } = "key.right";
        public static string Jump { get; } = "key.jump";
        public static string Sneak { get; } = "key.sneak";
        public static string Sprint { get; } = "key.sprint";
        public static string Drop { get; } = "key.drop";
        public static string Inventory { get; } = "key.inventory";
        public static string Chat { get; } = "key.chat";
        public static string Playerlist { get; } = "key.playerlist";
        public static string PickItem { get; } = "key.pickItem";
        public static string Command { get; } = "key.command";
        public static string Screenshot { get; } = "key.screenshot";
        public static string TogglePerspective { get; } = "key.togglePerspective";
        public static string SmoothCamera { get; } = "key.smoothCamera";
        public static string Fullscreen { get; } = "key.fullscreen";
        public static string SpectatorOutlines { get; } = "key.spectatorOutlines";
        public static string SwapHands { get; } = "key.swapHands";
        public static string SaveToolbarActivator { get; } = "key.saveToolbarActivator";
        public static string LoadToolbarActivator { get; } = "key.loadToolbarActivator";
        public static string Advancements { get; } = "key.advancements";
        public static string[] Hotbar { get; } = {
            "key.hotbar.1",
            "key.hotbar.2",
            "key.hotbar.3",
            "key.hotbar.4",
            "key.hotbar.5",
            "key.hotbar.6",
            "key.hotbar.7",
            "key.hotbar.8",
            "key.hotbar.9"
        };
    }

    class ChatKeybindComponent : ChatComponent
    {
        public Key Keybind { get; set; }

        public ChatKeybindComponent(Key keybind) : base()
        {
            Keybind = keybind;
        }

        public override Dictionary<string, object> ToSerializable()
        {
            Dictionary<string, object> root = base.ToSerializable();
            root.Add("keybind", Keybind);

            return root;
        }
    }
}

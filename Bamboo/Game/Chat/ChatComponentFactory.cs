using Bamboo.Protocol;
using System;
using System.Collections.Generic;

namespace Bamboo.Game.Chat
{
    class ChatComponentFactory
    {
        public static ChatComponent FromJSON(string json)
        {
            throw new NotImplementedException();
        }

        public static List<ChatTextComponent> FromString(string str)
        {
            List<ChatTextComponent> components = new List<ChatTextComponent>();
            StringBuffer buffer = new StringBuffer(str);

            while (buffer.Available > 0)
            {
                // In the case of resets being used, we might have several stacked components
                ChatTextComponent component = ParseString(buffer);

                components.Add(component);
            }

            return components;
        }

        private static ChatTextComponent ParseString(StringBuffer buffer)
        {
            return ParseString(buffer, null);
        }
        private static ChatTextComponent ParseString(StringBuffer buffer, ChatTextComponent parent)
        {
            /*
             * The below code recursively builds a ChatComponent with color/style inheritance working properly. For instance, if the color is set to blue and then the style is set to underlined,
             * then the underlined portion will be a child of the blue portion, so that it remains blue. The logic is as follows:
             *  - Read from the beginning, appending text characters to the Text field.
             *      - If the string starts with a formatting code, that will be applied immediately
             *  - As soon as a formatting code is encountered, save the component:
             *      - In most case, this means returning to the immediate parent
             *      - In the case of a reset formatting code, the loop at that level will complete, recursively bubbling up the effect until the parsing process is started again one what's levtover
             *  - Create a new component and continue reading, following this same pattern
             */

            // We work on just one component in each recursive call
            ChatTextComponent component = new ChatTextComponent();

            // State for the current component
            bool isEscaped = false; // Whether the current character is escaped by a previous character
            bool isFormatting = false; // Whether the current character is a formatting code

            // Gradually parse character-by-character
            while (buffer.Available > 0)
            {
                char c = buffer.ReadChar();

                // Check for formatting/escape characters
                if (!isEscaped)
                {
                    // The next character should be escaped
                    if (c == '\\')
                    {
                        isEscaped = true;
                        continue; // We're done with this special character
                    }

                    // Parse the formatting code
                    if (isFormatting)
                    {
                        if (c == 'r')
                        {
                            // We're done with this component, bubble up to the top level and let it start again
                            component.Reset = true;

                            return component;
                        }
                        else
                        {
                            isFormatting = false;

                            switch (c)
                            {
                                // TODO Check if parent already has color set?

                                // Color codes
                                case '0':
                                    component.Color = ChatComponentColor.Black;
                                    continue;
                                case '1':
                                    component.Color = ChatComponentColor.DarkBlue;
                                    continue;
                                case '2':
                                    component.Color = ChatComponentColor.DarkGreen;
                                    continue;
                                case '3':
                                    component.Color = ChatComponentColor.DarkCyan;
                                    continue;
                                case '4':
                                    component.Color = ChatComponentColor.DarkRed;
                                    continue;
                                case '5':
                                    component.Color = ChatComponentColor.Purple;
                                    continue;
                                case '6':
                                    component.Color = ChatComponentColor.Gold;
                                    continue;
                                case '7':
                                    component.Color = ChatComponentColor.Gray;
                                    continue;
                                case '8':
                                    component.Color = ChatComponentColor.DarkGray;
                                    continue;
                                case '9':
                                    component.Color = ChatComponentColor.Blue;
                                    continue;
                                case 'a':
                                    component.Color = ChatComponentColor.BrightGreen;
                                    continue;
                                case 'b':
                                    component.Color = ChatComponentColor.Cyan;
                                    continue;
                                case 'c':
                                    component.Color = ChatComponentColor.Red;
                                    continue;
                                case 'd':
                                    component.Color = ChatComponentColor.Pink;
                                    continue;
                                case 'e':
                                    component.Color = ChatComponentColor.Yellow;
                                    continue;
                                case 'f':
                                    component.Color = ChatComponentColor.White;
                                    continue;

                                // Text styles
                                case 'k':
                                    component.Style |= ChatComponentStyle.Obfuscated;
                                    continue;
                                case 'l':
                                    component.Style |= ChatComponentStyle.Bold;
                                    continue;
                                case 'm':
                                    component.Style |= ChatComponentStyle.Strikethrough;
                                    continue;
                                case 'n':
                                    component.Style |= ChatComponentStyle.Underlined;
                                    continue;
                                case 'o':
                                    component.Style |= ChatComponentStyle.Italic;
                                    continue;
                            }
                        }
                    }

                    if (c == Settings.ChatFormattingPrefix)
                    {
                        // Check if this is a new component or not
                        if (component.Text.Length > 0) {
                            buffer.Position--; // In order to capture the prefix in the recursive function, we have to move the cursor back a bit

                            // Determine whether to return (as a child) or to spawn a new child (as a parent)
                            if (parent != null)
                                return component;
                            else
                            {
                                ChatTextComponent child = ParseString(buffer, component);

                                // Custom flag is used to indicate that this component (and subcomponents) should be saved and then processing should restart on remaining text
                                if (child.Reset)
                                    return component;

                                component.Extra.Add(child);
                            }
                        }
                        else
                        {
                            isFormatting = true; // The next character is a formatting code
                        }
                        
                        continue;
                    }
                }

                // Append text to the text field of the current component
                component.Text += c;
                isEscaped = false;
            }

            return component;
        }
    }
}

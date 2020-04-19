﻿#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Game;
using Image = System.Drawing.Image;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public static class Converter
    {
        public static List<Player> PlayersFromTable(DataTable playersTable)
        {
            var playersList = new List<Player>();
            foreach (DataRow playerRow in playersTable.Rows)
            {
                playersList.Add(new Player(playerRow[$"{Column.Name}"].ToString(),
                                           playerRow[$"{Column.NickName}"].ToString(),
                                           Convert.ToInt32(playerRow[$"{Column.Id}"]),
                                           Base64ToBitmapImage(playerRow[$"{Column.Avatar}"].ToString())));
            }

            return playersList;
        }

        public static DateTime DateTimeFromString(string dateTimeStringFromDb)
        {
            return DateTime.Parse(dateTimeStringFromDb);
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public static string BitmapImageToBase64(BitmapImage bitmapImage)
        {
            var image = BitmapImageToBitmap(bitmapImage);
            return ImageToBase64(image, ImageFormat.Bmp);
        }

        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            if (bitmapImage.StreamSource != null)
            {
                return new Bitmap(bitmapImage.StreamSource);
            }

            if (bitmapImage.UriSource != null)
            {
                return new Bitmap(bitmapImage.UriSource.OriginalString);
            }

            return null; // todo
        }

        private static BitmapImage Base64ToBitmapImage(string base64String)
        {
            var image = (Bitmap) Base64ToImage(base64String);
            return BitmapToBitmapImage(image);
        }

        private static string ImageToBase64(Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                var imageBytes = ms.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        private static Image Base64ToImage(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes,
                                      0,
                                      imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = Image.FromStream(ms, true);
            return image;
        }

        public static Visibility ControlVisibilityToggle(Visibility visibility)
        {
            return visibility == Visibility.Visible
                       ? Visibility.Hidden
                       : Visibility.Visible;
        }

        public static GameTypeUi NewGameControlsToGameTypeUi(Grid mainWindowNewGameControls)
        {
            var selectedGameType = Enum.Parse<GameTypeUi>((((ComboBox) mainWindowNewGameControls
                                                                       .Children.OfType<FrameworkElement>()
                                                                       .Single(e => e.Name == "NewGameTypeComboBox")).SelectedItem as ComboBoxItem)
                                                          ?.Content.ToString());

            switch (selectedGameType)
            {
                case GameTypeUi.FreeThrowsSingle:
                    return GameTypeUi.FreeThrowsSingle;
                case GameTypeUi.FreeThrowsDouble:
                    return GameTypeUi.FreeThrowsDouble;
                case GameTypeUi.Classic:
                    return GameTypeUi.Classic;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static GameType NewGameControlsToGameTypeGameService(Grid mainWindowNewGameControls)
        {
            var selectedGameType = NewGameControlsToGameTypeUi(mainWindowNewGameControls);

            var selectedGamePoints = (((ComboBox) mainWindowNewGameControls
                                                  .Children.OfType<FrameworkElement>()
                                                  .Single(e => e.Name == "NewGamePointsComboBox")).SelectedItem as ComboBoxItem)
                                     ?.Content.ToString();

            switch (selectedGameType)
            {
                case GameTypeUi.FreeThrowsSingle:
                    switch (selectedGamePoints)
                    {
                        case "Free":
                            return GameType.FreeThrowsSingleFreePoints;
                        case "301":
                            return GameType.FreeThrowsSingle301Points;
                        case "501":
                            return GameType.FreeThrowsSingle501Points;
                        case "701":
                            return GameType.FreeThrowsSingle701Points;
                        case "1001":
                            return GameType.FreeThrowsSingle1001Points;
                    }

                    break;

                case GameTypeUi.FreeThrowsDouble:
                    switch (selectedGamePoints)
                    {
                        case "Free":
                            return GameType.FreeThrowsDoubleFreePoints;
                        case "301":
                            return GameType.FreeThrowsDouble301Points;
                        case "501":
                            return GameType.FreeThrowsDouble501Points;
                        case "701":
                            return GameType.FreeThrowsDouble701Points;
                        case "1001":
                            return GameType.FreeThrowsDouble1001Points;
                    }

                    break;

                case GameTypeUi.Classic:
                    switch (selectedGamePoints)
                    {
                        case "301":
                            return GameType.Classic301Points;
                        case "501":
                            return GameType.Classic501Points;
                        case "701":
                            return GameType.Classic701Points;
                        case "1001":
                            return GameType.Classic1001Points;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return GameType.FreeThrowsSingleFreePoints; // todo cant get here
        }

        public static int CamSetupSectorSettingValueToComboboxSelectedIndex(string camSetupSectorSettingValue)
        {
            switch (camSetupSectorSettingValue)
            {
                case "11":
                    return 0;
                case "11/14":
                    return 1;
                case "14":
                    return 2;
                case "14/9":
                    return 3;
                case "9":
                    return 4;
                case "9/12":
                    return 5;
                case "12":
                    return 6;
                case "12/5":
                    return 7;
                case "5":
                    return 8;
                case "5/20":
                    return 9;
                case "20":
                    return 10;
                case "20/1":
                    return 11;
                case "1":
                    return 12;
                case "1/18":
                    return 13;
                case "18":
                    return 14;
                case "18/4":
                    return 15;
                case "4":
                    return 16;
                case "4/13":
                    return 17;
                case "13":
                    return 18;
                case "13/6":
                    return 19;
                case "6":
                    return 20;
                case "6/10":
                    return 21;
                case "10":
                    return 22;
                case "10/15":
                    return 23;
                case "15":
                    return 24;
                case "15/2":
                    return 25;
                case "2":
                    return 26;
                case "2/17":
                    return 27;
                case "17":
                    return 28;
                case "17/3":
                    return 29;
                case "3":
                    return 30;
                case "3/19":
                    return 31;
                case "19":
                    return 32;
                case "19/7":
                    return 33;
                case "7":
                    return 34;
                case "7/16":
                    return 35;
                case "16":
                    return 36;
                case "16/8":
                    return 37;
                case "8":
                    return 38;
                case "8/11":
                    return 39;
                default:
                    return -1;
            }
        }

        public static string ComboBoxSelectedContentToString(ComboBox combobox)
        {
            return (combobox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        public static int ComboBoxSelectedContentToInt(ComboBox combobox)
        {
            return Convert.ToInt32((combobox.SelectedItem as ComboBoxItem)?.Content);
        }

        public static double ToDouble(string value)
        {
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        public static decimal ToDecimal(string value)
        {
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }

        public static float ToFloat(string value)
        {
            return (float) ToDecimal(value);
        }

        public static int ToInt(string value)
        {
            return Convert.ToInt32(value);
        }

        public static bool ToBool(string value)
        {
            return Convert.ToBoolean(value);
        }

        public static string ToString(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
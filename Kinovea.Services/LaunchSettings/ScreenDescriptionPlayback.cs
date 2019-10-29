﻿#region License
/*
Copyright © Joan Charmant 2012.
jcharmant@gmail.com 
 
This file is part of Kinovea.

Kinovea is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License version 2 
as published by the Free Software Foundation.

Kinovea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Kinovea. If not, see http://www.gnu.org/licenses/.

*/
#endregion
using System;
using System.Xml;

namespace Kinovea.Services
{
    public class ScreenDescriptionPlayback : IScreenDescription
    {
        public ScreenType ScreenType 
        {
            get { return ScreenType.Playback; }
        }
        
        /// <summary>
        /// Guid of the player screen into which this description should be reloaded.
        /// This is used to re-identify the autosave.kva after video load and restore metadata.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Full path to the video file to load.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Whether the video should start playing immediately after load.
        /// </summary>
        public bool Autoplay { get; set; }

        /// <summary>
        /// Speed at which to set the speed slider, whether the video is auto-play or not.
        /// </summary>
        public double SpeedPercentage { get; set; }

        /// <summary>
        /// Time origin of the video, used for synchronizing two videos.
        /// </summary>
        public long LocalSyncTime { get; set; }

        /// <summary>
        /// Whether the video should be stretched to fill the player screen estate.
        /// </summary>
        public bool Stretch { get; set; }

        /// <summary>
        /// Whether this screen is monitoring new files and loading them automatically.
        /// </summary>
        public bool IsReplayWatcher { get; set; } 

        
        public DateTime RecoveryLastSave { get; set; }
        
        public ScreenDescriptionPlayback()
        {
            SpeedPercentage = 100;
            Id = Guid.NewGuid();
            RecoveryLastSave = DateTime.MinValue;
        }

        public ScreenDescriptionPlayback(XmlReader reader) : this()
        {
            reader.ReadStartElement();

            while (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "FullPath":
                        FullPath = reader.ReadElementContentAsString();
                        break;
                    case "Autoplay":
                        Autoplay = XmlHelper.ParseBoolean(reader.ReadElementContentAsString());
                        break;
                    case "SpeedPercentage":
                        SpeedPercentage = reader.ReadElementContentAsDouble();
                        break;
                    case "Stretch":
                        Stretch = XmlHelper.ParseBoolean(reader.ReadElementContentAsString());
                        break;
                    case "IsReplayWatcher":
                        IsReplayWatcher = XmlHelper.ParseBoolean(reader.ReadElementContentAsString());
                        break;
                    default:
                        reader.ReadOuterXml();
                        break;
                }
            }

            reader.ReadEndElement();
        }
    }
}

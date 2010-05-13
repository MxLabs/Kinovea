﻿#region License
/*
Copyright © Joan Charmant 2008-2009.
joan.charmant@gmail.com 
 
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
using Kinovea.ScreenManager.Languages;
using System;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace Kinovea.ScreenManager
{
	/// <summary>
	/// FormProgressBar is a simple form to display a progress bar.
	/// The progress is computed outside and communicated through Update() method.
	/// See AbstractVideoFilter for usage sample.
	/// </summary>
	public partial class formProgressBar : Form
	{
		#region Callbacks
		public delegate void CancelHandler(object sender);
		public Kinovea.ScreenManager.formProgressBar.CancelHandler Cancel;
		#endregion
		
		#region Members
		private bool m_IsIdle;
		#endregion
		
		#region Constructor
		public formProgressBar(bool _IsCancellable)
		{
			InitializeComponent();
			Application.Idle += new EventHandler(IdleDetector);
			btnCancel.Visible = _IsCancellable;
			
            // Culture
            this.Text = "   " + ScreenManagerLang.FormProgressBar_Title;
			labelInfos.Text = ScreenManagerLang.FormFileSave_Infos + " 0 / ~?";
			btnCancel.Text = ScreenManagerLang.Generic_Cancel;
		}
		#endregion	
		
		#region Methods
		private void IdleDetector(object sender, EventArgs e)
        {
            m_IsIdle = true;
        }
		public void Update(int _iValue, int _iMaximum, bool _bAsPercentage)
		{
			if (m_IsIdle)
            {
                m_IsIdle = false;
                
                progressBar.Maximum = _iMaximum;
                progressBar.Value = _iValue > 0 ? _iValue : 0;
                
                if(_bAsPercentage)
                {
                	labelInfos.Text = ScreenManagerLang.FormFileSave_Infos + " " + (int)((_iValue * 100) / _iMaximum) + "%";
                }
				else
				{
					labelInfos.Text = ScreenManagerLang.FormFileSave_Infos + " " + _iValue + " / ~" + _iMaximum;
				}
			}
		}
		#endregion
		
		#region Events
		private void formProgressBar_FormClosing(object sender, FormClosingEventArgs e)
		{
			Application.Idle -= new EventHandler(IdleDetector);	
		}
		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			// User clicked on cancel, trigger the callback that will cancel the ongoing operation.
			if(Cancel != null) 
				Cancel(this);
			
			btnCancel.Enabled = false;
		}
		#endregion
		
	}
}

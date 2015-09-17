using Loltimer.ChampionTimer;
using System;
using System.Collections.Generic;
//
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Loltimer
{
    public class DragAndDrop
    {

        private bool _isDown;
        private bool _isDragging;
        private Point _startPoint;
        private UIElement _realDragSource;
        private UIElement _dummyDragSource = new UIElement();
        private StackPanel sp;
        private Ellipse[] ellipses = new Ellipse[5];

        public DragAndDrop(StackPanel stackPanel, List<ChampionTimerPresenter> champPanels)
        {
            //add listeners for drag and drop

            for (int i = 0; i < GlobalVars.numOfPanels; i++)
            {
                ellipses[i] = champPanels[i].GetChampionTimerView().GetEllipse();
                ellipses[i].PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown_OnEllipse;
                ellipses[i].PreviewMouseLeftButtonUp += PreviewMouseLeftButtonUp_OnEllipse;
                string path = Directory.GetCurrentDirectory();
                ellipses[i].Cursor = new Cursor(File.Open("..\\..\\Icons\\openhand.cur", FileMode.Open));
                ellipses[i].DragEnter += DragEnter_OnEllipse;

            }
            this.sp = stackPanel;
            this.sp.PreviewMouseMove += sp_PreviewMouseMove;
            this.sp.Drop += Drop_Action_OnStackPanel;
        }

        private void PreviewMouseLeftButtonDown_OnEllipse(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == this.sp)
            {
            }
            else
            {
                _isDown = true;
                _startPoint = e.GetPosition(this.sp);
            }
        }

        private void PreviewMouseLeftButtonUp_OnEllipse(object sender, MouseButtonEventArgs e)
        {
            _isDown = false;
            _isDragging = false;
            if (_realDragSource != null)
                _realDragSource.ReleaseMouseCapture();
        }

        private void sp_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) && ((Math.Abs(e.GetPosition(this.sp).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(this.sp).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    _isDragging = true;
                    _realDragSource = e.Source as UIElement;
                    _realDragSource.CaptureMouse();
                    DragDrop.DoDragDrop(_dummyDragSource, new DataObject("UIElement", e.Source, true), DragDropEffects.Move);
                }
            }
        }

        private void DragEnter_OnEllipse(object sender, DragEventArgs e)
        {
            var x = e.Data.GetFormats();
            if (e.Data.GetDataPresent("UIElement"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void Drop_Action_OnStackPanel(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UIElement"))
            {

                UIElement droptarget = e.Source as UIElement;
                int movingItemIndex = this.sp.Children.IndexOf(_realDragSource);
                int droptargetIndex = -1, i = 0;
                foreach (UIElement element in this.sp.Children)
                {
                    if (element.Equals(droptarget))
                    {
                        droptargetIndex = i;
                        break;
                    }
                    i++;
                }

                UIElement kickedOutTarget = this.sp.Children[i];
                if (droptargetIndex != -1)
                {
                    this.sp.Children.Remove(_realDragSource);
                    this.sp.Children.Insert(droptargetIndex, _realDragSource);
                    this.sp.Children.Remove(kickedOutTarget);
                    this.sp.Children.Insert(movingItemIndex, kickedOutTarget);
                }



                _isDown = false;
                _isDragging = false;
                _realDragSource.ReleaseMouseCapture();
            }
        }
    }
}


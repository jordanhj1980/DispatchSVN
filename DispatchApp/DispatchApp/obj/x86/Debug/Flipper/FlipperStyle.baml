   M S B A M L   `   `   `  ����F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DispatchApp.wpfDispatchApp.wpf  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089-   'System.Windows.Media.Animation.SineEase   FlipEase7  1System.Windows.Media.Animation.EasingFunctionBase    
EasingMode   DispatchApp.wpf.Flipper �� 
TargetType-  'System.Windows.Input.KeyboardNavigation �� Property �� Value �� 
TargetType ��Name'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition   From		   To
   
Storyboard g� FillBehavior g� Duration �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� Value O� KeyTime   EasingFunction  System.Windows.Visibility   Visible R� Value R� KeyTime   	Collapsed   States 	  System.Windows.VisualState 	  
Storyboard
   DispatchApp.wpf.Plane3D 
 Name 
  	RotationY 
  ZFactor 
  Child ��Name   FrontContent   FrontContentTemplate#   FrontContentTemplateSelector    FrontContentStringFormat! �� ContentStringFormat" J� RenderTransformOrigin#   BackContent$   BackContentTemplate"%   BackContentTemplateSelector&   BackContentStringFormat�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      %wpfclr-namespace:DispatchApp.wpf  5      %�
  &	        '  #     7   7   7   7   7   7   7   7      5      $  	EaseInOut=�6    �� 5      "  6   .�5   
   ��  �@ 	IsTabStop .  69   �� 5	   
    � ��$	 Top=�6.   �� 5
   
    � ��$
 Left=�60   �� 5   
    � �� 5      �� 5      "  6"   .R�5      �� $ Border��5      #��z��5      #��z��6G   #��z��6s    5        5      
FlipStates�� 5   "     5   &   $ 	Unflipped��67   $	 Flipped��6H   .
 5   *   �� $ HoldEnd=�65   .��5   .   U� $ 0:0:0.4D�6L   $ 	RotationY�6_   $ Plane3D��6�   .\�5   2     $ 0Q�6G   $ 0:0:0.0��6Q   8   6c     5   2   $	 -90Q�6G   $ 0:0:0.2��6S   8  6e     5   2   $ 90Q�6G   $ 0:0:0.2��6R   8  6d     5   2   $ 0Q�6G   $ 0:0:0.4��6Q   8  6c   5   /   T� 5   .   $ 
Visibility�6M   $ BackContentPresenter��6t   ..�5   2   i� # Z 6I   $ 0:0:0.2��6o   5   /   T� 5   .   $ 
Visibility�6M   $ FrontContentPresenter��6t   ..�5   2   i� # Z 6I   $ 0:0:0.2��6q   5    /   5!   +   5"   '     5#   &   $ Flipped��67   $	 	Unflipped��6F   .
 5$   *   �� $ HoldEnd=�65   .��5%   .   U� $ 0:0:0.4D�6L   $ 	RotationY�6_   $ Plane3D��6�   .\�5&   2     $ 0Q�6G   $ 0:0:0.0��6Q   8  6c     5'   2   $ 90Q�6G   $ 0:0:0.2��6R   8  6d     5(   2   $	 -90Q�6G   $ 0:0:0.2��6S   8  6e     5)   2   $ 0Q�6G   $ 0:0:0.4��6Q   8  6c   5*   /   T� 5+   .   $ 
Visibility�6M   $ BackContentPresenter��6t   ..�5,   2   i� # Z 6I   $ 0:0:0.2��6q   5-   /   T� 5.   .   $ 
Visibility�6M   $ FrontContentPresenter��6t   ..�5/   2   i� # Z 6I   $ 0:0:0.2��6o   50   /   51   +   52   '   . 54   "   	  	Unflipped��. 55   &   �� .��56   *   T� $ 
Visibility�6I   $ BackContentPresenter��6p   ..�57   .   i� # Z 6E   58   +   T� 59   *   $ 
Visibility�6I   $ FrontContentPresenter��6p   ..�5:   .   i� # Z 6E   5;   +   5<   '   5=   #   	  5>   "   Flipped��. 5?   &   �� .��5@   *   T� $ 
Visibility�6I   $ BackContentPresenter��6p   ..�5A   .   i� # Z 6E   5B   +   T� 5C   *   $ 
Visibility�6I   $ FrontContentPresenter��6p   ..�5D   .   i� # Z 6E   5E   +   5F   '   5G   #   5H      ._�5J      
  $ Plane3D��6&   $ 0Q�67   $ 2.055Q�6E   . 5K      � .+�5L   "   �� $ FrontContentPresenter��5M   %   #��z��5N   %   #��z 5O   %   #��z 5P   %   #��z 5Q   %   #! z  5R   %   $" .5,.5&�5S   %   �� 5T   "   $ BackContentPresenter��5U   %   $y�Hidden=�5V   %   #��z��5W   %   #��z# 5X   %   #��z$ 5Y   %   #��z% 5Z   %   #! z& 5[   %   $" .5,.5&�5\   %   5]      5^      5_      5`      5b      5c      5e      
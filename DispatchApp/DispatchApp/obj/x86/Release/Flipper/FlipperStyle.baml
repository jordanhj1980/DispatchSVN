   M S B A M L   `   `   `  ���� F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DispatchApp.wpfDispatchApp.wpf  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089-   'System.Windows.Media.Animation.SineEase   FlipEase7  1System.Windows.Media.Animation.EasingFunctionBase    
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
  Child ��Name   FrontContent   FrontContentTemplate#   FrontContentTemplateSelector    FrontContentStringFormat! �� ContentStringFormat" J� RenderTransformOrigin#   BackContent$   BackContentTemplate"%   BackContentTemplateSelector&   BackContentStringFormat�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     %wpfclr-namespace:DispatchApp.wpf  %D  &	        '       7   7   7   7   7   7   7   7      $  	EaseInOut=��� "  .���  �@ 	IsTabStop .  ��  � ��$	 Top=���  � ��$
 Left=���  � �� �� "  .R��� $ Border��#��z��#��z��#��z��   
FlipStates��   $ 	Unflipped��$	 Flipped��.
 �� $ HoldEnd=�.��U� $ 0:0:0.4D�$ 	RotationY�$ Plane3D��.\�  $ 0Q�$ 0:0:0.0��8     $	 -90Q�$ 0:0:0.2��8    $ 90Q�$ 0:0:0.2��8    $ 0Q�$ 0:0:0.4��8  T� $ 
Visibility�$ BackContentPresenter��..�i� # Z $ 0:0:0.2��T� $ 
Visibility�$ FrontContentPresenter��..�i� # Z $ 0:0:0.2��  $ Flipped��$	 	Unflipped��.
 �� $ HoldEnd=�.��U� $ 0:0:0.4D�$ 	RotationY�$ Plane3D��.\�  $ 0Q�$ 0:0:0.0��8    $ 90Q�$ 0:0:0.2��8    $	 -90Q�$ 0:0:0.2��8    $ 0Q�$ 0:0:0.4��8  T� $ 
Visibility�$ BackContentPresenter��..�i� # Z $ 0:0:0.2��T� $ 
Visibility�$ FrontContentPresenter��..�i� # Z $ 0:0:0.2��. 	  	Unflipped��. �� .��T� $ 
Visibility�$ BackContentPresenter��..�i� # Z T� $ 
Visibility�$ FrontContentPresenter��..�i� # Z 	  Flipped��. �� .��T� $ 
Visibility�$ BackContentPresenter��..�i� # Z T� $ 
Visibility�$ FrontContentPresenter��..�i� # Z ._�
  $ Plane3D��$ 0Q�$ 2.055Q�. � .+��� $ FrontContentPresenter��#��z��#��z #��z #��z #! z  $" .5,.5&��� $ BackContentPresenter��$y�Hidden=�#��z��#��z# #��z$ #��z% #! z& $" .5,.5&�
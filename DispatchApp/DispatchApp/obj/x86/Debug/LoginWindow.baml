   M S B A M L   `   `   `  ����V  RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	    DispatchApp   DispatchApp.LoginWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5   	   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf   5   	   3system&clr-namespace:System;assembly=mscorlib   5   	   -   5          � WindowStyle$
  None=�5   	    � AllowsTransparency .    MaterialDesignPaper#���   5   	   $��SimHei)�5	   	    � Title$ LoginWindow��5
   	   $
��1080��6   $
��1920��6+    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ MaterialDictionary.xaml?�6%   %    5      .��5      (� .��5   
   � $
��1080��6   $
��1920��6    � RowDefinitions 5      �� 5      $��* �6    �� 5      $��3* �6    .+�5      #   MaterialDesignThemes.Wpf.Card  +   %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�6"     PrimaryHueDarkBrush#���  5       ! PrimaryHueDarkForegroundBrush#���  5      $	��150��5      $
��1920��6    $��8q�5         UniformCornerRadius$ 0Q�5       .��5!      �� $��60(�6   $��Center=�6*   $��Center=�6G   !智 能 调 度 台 客 户 端5#      5$        5&      -   5        	  Name$	 loginBox��5&   "   $��1��64   $	��500��6A   $	��720��6M   $ Depth5=�6Z   .��5(      � 
 +� FocusedElement
 6   ��  �� ElementName$ 
TxUserName�� 5)      �� 5*      $	��300 �6(   �� 5+      $
��Auto �6(   �� 5,      $
��Auto �6(   �� 5-      $
��Auto �6(   �� 5.      $��20 �6(   �� 5/      $��50 �6(   �� 50      $��50 �6(   �� 51      $��* �6(   .+�53      �� $%��Resources/startup_1920x1080.jpg��6   $
��Fill=�6E   �� 54      -   5        $��
TxUserName��54   +   $��1��6   
������� % !MaterialDesignFloatingHintTextBox#��[ 55      $��100,10,100,0q�56      $	��Top=�57      $��90��58       �� AcceptsReturn .  )   #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	用户名��5:      $��20(�6:   G� 5<      -   5        $��
TxPassword��5<   /   $��2��6"   
�������$��100,10,100,0q�5>       ) %MaterialDesignFloatingHintPasswordBox#��[ 5?      $��Bottom=�5@      $��90��5A      $ 密码��5B      $��20(�69   �� 5D      $��3��6!    �� Orientation$ 
Horizontal=�6.   $��0,40,0,0q�6G   $��Center=�6Y   $��Bottom=�6v   .+�5E      �� -   5        $��0,0,45,0q�5E   "   $ Depth5=�64     MaterialDesignRaisedButton#��[ 5F       �� 	IsDefault . $��50��5I      $	��100��6)   $��24(�65   .��6C   登录5K      �� 5M      -   5        $ Depth5=�5M   !   $��50,0,0,0q�6R   #��[ 5N      $	�� 50��5P      $	��100��6*   $��24(�66   .��6E   退出5R      5S      '   !MaterialDesignThemes.Wpf.Snackbar  5T      -   5          Name$ SnackbarOne��5X      $��Stretch=�5U      $	��Top=�67   $��1��5V      
��� E��
�������   MessageQueue 5W      4   .MaterialDesignThemes.Wpf.MessageQueueExtension  5X      �� 5Z      -   5        $��btn_adv��5Z   *   $��3��6   $��Center=�69   $��25(�5[      $��0q�6+   
������ $��62��6X   $
��Auto��6c   $��215,86,223,45.2q�6q   $��3��6�   .��5\      �� -	   5        $��	image_adv��5\       $��Resources/adv_expand.png��62   $
��Left=�6T   $
��Fill=�6o   5]      �� 5^      -
   5        $��	serialBox��5^   !   $ 
Horizontal=�62   $��5��6K   $
��Left=�6X   $��20,0,0,0q�6s   .+�5_      �� $��注册码：��6$   $��Right=�60   $��24(�6L   
�������$	��106��6m   $��Center=�6y   $��20,0,5,0q�6�   �� 5`      -   5         ��Name$ serial��5`   "   
�������$	��231��5a   !   $��Center=�6-   $
��Left=�6H   $��20(�6c   �� 5b      -   5        
�������$��注册��5b   4   $��10,0,0,0q�6X   $��72��5c   !   $��Center=�6,   $
��Left=�6G   $��20(�6b   5d      �� 5e      -   5        $��advBox��5e   !   $ 
Horizontal=�6/   $��6��6H   $
��Left=�6U   $��20,0,0,0q�6p   .+�5f      �� $��IP地址：��6$   $��Right=�61   $��24(�6M   
�������$	��106��6n   $��Center=�6z   $��20,0,5,0q�6�   �� 5g      -   5        $ IPAddr��5g   "   
�������$	��231��5h   !   $��Center=�6-   $
��Left=�6H   $��24(�6c   5i      5k      5l      5n      5o      5p      
   M S B A M L   `   `   `  ����F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp  5   	   V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters 5	   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5
   	       DispatchApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   +localclr-namespace:DispatchApp     5   	   FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf  5	   	   3system&clr-namespace:System;assembly=mscorlib  5
   	   :d2http://schemas.microsoft.com/expression/blend/2008  5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	     � Title$  
MainWindow��5   	   $
��1080��6   $
��1920��6*    � 
ResizeMode$ CanMinimize=�67   $��0q�5   	    � Cursor$ Arrow��5   e   $	��Top=�6t   $
��Auto=�6�    � IsSharedSizeScope .  � WindowStyle$
 None=�5   	    � AllowsTransparency .  � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Resources/Generic.xaml?�6%   �� 5      $ MaterialDictionary.xaml?�6%   %    5      ��5      �� 5   
   	 �� ImageSource$	 Resources/main_1920_2.png��6   
 j� Stretch$

 Fill=�6=   .��5      (� .��5   
   � -   5        $��outframe��5   8   $
��left=�6   $
��1920��6+   $��0,0,0,0q�6H    J� RenderTransformOrigin$ 0.5,0.5&�6Y   $
��1080��6y    5       #   DispatchApp.ComeCallConverter   comeCallDate��%   &	          5!      6*   5"       � RowDefinitions 5#      �� 5$      $��38 �6    �� 5%      $
��100* �6    �� 5&      $
��904* �6    �� 5'      $��38 �6    .+�5*      � ���#000000$��0��6(   $��0,0,10,0q�65   $��7��6H   .+�5+      �� 
�������$��15,0,0,0q�5,      $
��Left=�5-      $��Center=�5.      $��20(�5/      $��调度台LOGO��50      �� 51      -   5        $��
label_Date��53      $��Label��51      $��169,0,1587.667,4q�52      $��Center=�54      $��20(�55      $ 
0.511,1.75&�56      $��32��57      $��Bottom=�58      � 59      $��38��6   $��Right=�5:      $	��Top=�5;       � Orientation$ 
Horizontal=�5<      $��0,0,-0.333,0q�6/   $	��443��6E   .+�5=      o� ���#000
�������$��20(�6@   $��Center=�6N   $y�Hidden=�6i   .@�5>      m� $��系统��6#   .@�5@      m� -   5        $��menuOpen��5@   '   $
��Open��67   ���#0006q   m� 5A      $
��Save��6'   ���#0006I   m� 5B      $��Delete��6'   ���#0006K   5C      m� 5D      $��视图��6#   61   m� 5E      $��工具��6#   61   m� 5F      $��高级��6#   61   m� 5G      $��帮助��6#   61   5H      5J      5K      �� 5N      -   5        $��label1��5N      $	��100��5O      $
��1920��6   $��1��5P      $
��Left=�5Q      $	��Top=�5R      $��7��5S        PrimaryHueDarkBrush#���  5T      .��5U      ��  �� Orientation$ 
Horizontal=�5V      $
��1920��5W      .+�5X      �� -   5        $��
label_Time��5X      $��Label��5Y      $��46(�5Z      $��Center=�5[      $��Center=�6>   $
��Left=�5\      $	��285��64   �� 5]      -   5         ��Name$ 	sw_button��5]      
������ $��0��6I   $��92��6Y   $��Center=�6e   $	��Top=�6�   $	��102��6�   $	��0,0q�5^       J� Opacity$ 1Q�6*   ��66   Z� .��5_      �� $��90��6%   $��70��61   .+�5a      '  !MaterialDesignThemes.Wpf.PackIcon  -   5        $��icon_sw��5a   6   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ ImportantDevices=�6E   $��60��6]   $��60��6h   $��Center=�6t   $��5,0,0,0q�6�   �� 5b       �� TextAlignment$ Center=�6(   $	��100��6F   $��	软交换��6R   $��Center=�6]   $��-15,0,-15.667,0q�6z   $��22(�6�   $��SimHei)�6�   $��Center=�6�   $��23��6�   5c      5e      �� 5f      -   5        $ user_button��5f      
������ $��0��6K   $��92��6[   $��Center=�6g   $	��Top=�6�   $	��102��6�   $	��0,0q�5g      $ 1Q�6*   ��66   Z� .��5h      �� $	��100��6%   $��69��62   .+�5j        -	   5        $��	icon_user��5j   6   $
 User=�6H   $��60��6T   $��60��6_   $��Center=�6k   $��5,0,0,0q�6�   �� 5k      $ Center=�6(   $	��100��6F   $��用户��6R   $��Center=�6\   $��0,0,0,0q�6y   $��22(�6�   $��SimHei)�6�   $��Center=�6�   5l      5m      �� 5n      -
   5        $ desk_button��5n      
������ $��0��6K   $��92��6[   $��Center=�6g   $	��Top=�6�   $	��102��6�   $	��0,0q�5o      $ 1Q�6*   ��66   Z� .��5p      �� $��92��6%   $��69��61   .+�5r        -   5        $��icon_key��5r   6   $ Keyboard=�6G   $��60��6W   $��60��6b   $��Center=�6n   $��5,0,0,0q�6�   �� 5s      $ Center=�6(   $	��100��6F   $��	调度台��6R   $��Center=�6]   $��0,0,0,0q�6z   $��22(�6�   $��SimHei)�6�   $��Center=�6�   5t      5u      �� 5v      -   5        $ contact_button��5v      
������ $��0��6N   $��92��6^   $��Center=�6j   $	��Top=�6�   $	��102��6�   $	��0,0q�5w      $ 1Q�6*   ��66   Z� .��5x      �� $��92��6%   $��69��61   .+�5z        -   5        $��icon_contact��5z   6   $ ContactMail=�6K   $��60��6^   $��60��6i   $��Center=�6u   $��5,0,0,0q�6�   �� 5{      $ Center=�6(   $	��100��6F   $��	电话簿��6R   $��Center=�6]   $��0,0,0,0q�6z   $��22(�6�   $��SimHei)�6�   $��Center=�6�   5|      5}      �� 5~      -   5        $��ShowKeyLabel��5~      $��Center=�5      $��46(�5�      $��98��5�      $	��800��6%   $��Bottom=�5�      �� 5�      -   5        $	��100��5�      $��Center=�6*   $	��Top=�6`   $	��107��6x   $	��8,0q�6�   ��6�   Z� .��5�      �� $	��100��6%   $��80��62   .+�5�      �� $��Resources/lock_close.png��6$   $��58��6F   $��58��6R   $	��Top=�6^   �� 5�      $ Center=�6(   $��80��6F   $��锁定��6Q   $��Center=�6[   $��0,0,0,0q�6x   $��24(�6�   $��SimHei)�6�   $��Center=�6�   5�      5�      �� 5�      -   5        $��image_photo��5�      $��84��5�      $��84��6%   $
��Left=�5�      $
��Fill=�5�      $��Center=�5�      $0��*/DispatchApp;component/Resources/photo.png��5�      �� 5�      -   5        $ button3_Copy��5�      $��注销��5�      $��30(�6&   $��SimHei)�64   $��44��5�      $	��100��6%   $
��Left=�5�      $��Center=�65   ��5�      Z� ��60   Z� 5�      5�      �� 5�      $��3��6   $ 
Horizontal=�6&   .+�5�      �� $��服务器IP:��6   ���	#FFD8D7D7$��22(�6@   $
��Left=�6N   $	��109��6i   $��	0,0,0,0.4q�6u   �� 5�      -   5        $��serverIP��5�      $��	127.0.0.1��6,   $��Center=�6=   ���	#FFD8D7D7$��22(�6o   $
��Left=�6}   $	��277��6�   5�      �� 5�      -   5        $��CenterPanel2��5�      $��2��61   )  #MaterialDesignThemes.Wpf.DialogHost  5�      -   5           
Identifier$ 
MessageBox��5�   (   $��0��6@   $��1��6P   $��0��6d   $��5��6q      SnackbarMessageQueue 5�      ��  �� ElementName$ MainSnackbar�� �� Path$ MessageQueue�5�      5�      5�      
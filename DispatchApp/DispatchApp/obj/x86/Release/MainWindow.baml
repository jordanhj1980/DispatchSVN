   M S B A M L   `   `   `  ���� F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp  V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem     DispatchApp.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     +localclr-namespace:DispatchApp     FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf  3system&clr-namespace:System;assembly=mscorlib  :d2http://schemas.microsoft.com/expression/blend/2008  Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006    � Title$  
MainWindow��$
��1080��$
��1920�� � 
ResizeMode$ CanMinimize=�$��0q� � Cursor$ Arrow��$	��Top=�$
��Auto=� � IsSharedSizeScope .  � WindowStyle$
 None=� � AllowsTransparency .  � 	Resources  �� MergedDictionaries �� Source��  �� $ Resources/Generic.xaml?��� $ MaterialDictionary.xaml?�%    ���� 	 �� ImageSource$	 Resources/main_1920_2.png��
 j� Stretch$

 Fill=�.��(� .��� -   $��outframe��$
��left=�$
��1920��$��0,0,0,0q� J� RenderTransformOrigin$ 0.5,0.5&�$
��1080�� #   DispatchApp.ComeCallConverter   comeCallDate��%   &	           � RowDefinitions �� $��38 ��� $
��100* ��� $
��904* ��� $��38 �.+�� ���#000000$��0��$��0,0,10,0q�$��7��.+��� 
�������$��15,0,0,0q�$
��Left=�$��Center=�$��20(�$��调度台LOGO���� -   $��
label_Date��$��Label��$��169,0,1587.667,4q�$��Center=�$��20(�$ 
0.511,1.75&�$��32��$��Bottom=�� $��38��$��Right=�$	��Top=� � Orientation$ 
Horizontal=�$��0,0,-0.333,0q�$	��443��.+�o� ���#000
�������$��20(�$��Center=�$y�Hidden=�.@�m� $��系统��.@�m� -   $��menuOpen��$
��Open�����#000m� $
��Save�����#000m� $��Delete�����#000m� $��视图��m� $��工具��m� $��高级��m� $��帮助���� -   $��label1��$	��100��$
��1920��$��1��$
��Left=�$	��Top=�$��7��  PrimaryHueDarkBrush#���  .����  �� Orientation$ 
Horizontal=�$
��1920��.+��� -   $��
label_Time��$��Label��$��46(�$��Center=�$��Center=�$
��Left=�$	��285���� -    ��Name$ 	sw_button��
������ $��0��$��92��$��Center=�$	��Top=�$	��102��$	��0,0q� J� Opacity$ 1Q���Z� .���� $��90��$��70��.+�'  !MaterialDesignThemes.Wpf.PackIcon  -   $��icon_sw���  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ ImportantDevices=�$��60��$��60��$��Center=�$��5,0,0,0q���  �� TextAlignment$ Center=�$	��100��$��	软交换��$��Center=�$��-15,0,-15.667,0q�$��22(�$��SimHei)�$��Center=�$��23���� -   $ user_button��
������ $��0��$��92��$��Center=�$	��Top=�$	��102��$	��0,0q�$ 1Q���Z� .���� $	��100��$��69��.+�  -	   $��	icon_user��$
 User=�$��60��$��60��$��Center=�$��5,0,0,0q��� $ Center=�$	��100��$��用户��$��Center=�$��0,0,0,0q�$��22(�$��SimHei)�$��Center=��� -
   $ desk_button��
������ $��0��$��92��$��Center=�$	��Top=�$	��102��$	��0,0q�$ 1Q���Z� .���� $��92��$��69��.+�  -   $��icon_key��$ Keyboard=�$��60��$��60��$��Center=�$��5,0,0,0q��� $ Center=�$	��100��$��	调度台��$��Center=�$��0,0,0,0q�$��22(�$��SimHei)�$��Center=��� -   $ contact_button��
������ $��0��$��92��$��Center=�$	��Top=�$	��102��$	��0,0q�$ 1Q���Z� .���� $��92��$��69��.+�  -   $��icon_contact��$ ContactMail=�$��60��$��60��$��Center=�$��5,0,0,0q��� $ Center=�$	��100��$��	电话簿��$��Center=�$��0,0,0,0q�$��22(�$��SimHei)�$��Center=��� -   $��ShowKeyLabel��$��Center=�$��46(�$��98��$	��800��$��Bottom=��� -   $	��100��$��Center=�$	��Top=�$	��107��$	��8,0q���Z� .���� $	��100��$��80��.+��� $��Resources/lock_close.png��$��58��$��58��$	��Top=��� $ Center=�$��80��$��锁定��$��Center=�$��0,0,0,0q�$��24(�$��SimHei)�$��Center=��� -   $��image_photo��$��84��$��84��$
��Left=�$
��Fill=�$��Center=�$0��*/DispatchApp;component/Resources/photo.png���� -   $ button3_Copy��$��注销��$��30(�$��SimHei)�$��44��$	��100��$
��Left=�$��Center=���Z� ��Z� �� $��3��$ 
Horizontal=�.+��� $��服务器IP:�����	#FFD8D7D7$��22(�$
��Left=�$	��109��$��	0,0,0,0.4q��� -   $��serverIP��$��	127.0.0.1��$��Center=����	#FFD8D7D7$��22(�$
��Left=�$	��277���� -   $��CenterPanel2��$��2��)  #MaterialDesignThemes.Wpf.DialogHost  -      
Identifier$ 
MessageBox��$��0��$��1��$��0��$��5��   SnackbarMessageQueue ��  �� ElementName$ MainSnackbar�� �� Path$ MessageQueue�
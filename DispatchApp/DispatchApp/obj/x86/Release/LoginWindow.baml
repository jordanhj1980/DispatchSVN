   M S B A M L   `   `   `  ���� V  RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  DispatchApp   DispatchApp.LoginWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf   3system&clr-namespace:System;assembly=mscorlib   -     � WindowStyle$
  None=� � AllowsTransparency .    MaterialDesignPaper#���   $��SimHei)� � Title$ LoginWindow��$
��1080��$
��1920�� � 	Resources  �� MergedDictionaries �� Source��  �� $ MaterialDictionary.xaml?�%    .��(� .��� $
��1080��$
��1920�� � RowDefinitions �� $��* ��� $��3* �.+�#   MaterialDesignThemes.Wpf.Card  +   %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�  PrimaryHueDarkBrush#���   ! PrimaryHueDarkForegroundBrush#���  $	��150��$
��1920��$��8q�   UniformCornerRadius$ 0Q�.���� $��60(�$��Center=�$��Center=�!智 能 调 度 台 客 户 端  -   	  Name$	 loginBox��$��1��$	��500��$	��720��$ Depth5=�.��� 
 +� FocusedElement
 ��  �� ElementName$ 
TxUserName�� �� $	��300 ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��20 ��� $��50 ��� $��50 ��� $��* �.+��� $%��Resources/startup_1920x1080.jpg��$
��Fill=��� -   $��
TxUserName��$��1��
������� % !MaterialDesignFloatingHintTextBox#��[ $��100,10,100,0q�$	��Top=�$��90�� �� AcceptsReturn .  )   #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	用户名��$��20(�G� -   $��
TxPassword��$��2��
�������$��100,10,100,0q� ) %MaterialDesignFloatingHintPasswordBox#��[ $��Bottom=�$��90��$ 密码��$��20(��� $��3�� �� Orientation$ 
Horizontal=�$��0,40,0,0q�$��Center=�$��Bottom=�.+��� -   $��0,0,45,0q�$ Depth5=�  MaterialDesignRaisedButton#��[  �� 	IsDefault . $��50��$	��100��$��24(�.��登录�� -   $ Depth5=�$��50,0,0,0q�#��[ $	�� 50��$	��100��$��24(�.��退出'   !MaterialDesignThemes.Wpf.Snackbar  -     Name$ SnackbarOne��$��Stretch=�$	��Top=�$��1��
��� E��
�������   MessageQueue 4   .MaterialDesignThemes.Wpf.MessageQueueExtension  �� -   $��btn_adv��$��3��$��Center=�$��25(�$��0q�
������ $��62��$
��Auto��$��215,86,223,45.2q�$��3��.���� -	   $��	image_adv��$��Resources/adv_expand.png��$
��Left=�$
��Fill=��� -
   $��	serialBox��$ 
Horizontal=�$��5��$
��Left=�$��20,0,0,0q�.+��� $��注册码：��$��Right=�$��24(�
�������$	��106��$��Center=�$��20,0,5,0q��� -    ��Name$ serial��
�������$	��231��$��Center=�$
��Left=�$��20(��� -   
�������$��注册��$��10,0,0,0q�$��72��$��Center=�$
��Left=�$��20(��� -   $��advBox��$ 
Horizontal=�$��6��$
��Left=�$��20,0,0,0q�.+��� $��IP地址：��$��Right=�$��24(�
�������$	��106��$��Center=�$��20,0,5,0q��� -   $ IPAddr��
�������$	��231��$��Center=�$
��Left=�$��24(�
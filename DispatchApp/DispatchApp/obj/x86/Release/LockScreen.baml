   M S B A M L   `   `   `  ���� F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DispatchApp.wpfDispatchApp.wpf      DispatchApp.LockScreen   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  )localclr-namespace:DispatchApp.wpf    � WindowStyle$
  None=� � AllowsTransparency .    MaterialDesignPaper#���     MaterialDesignFont#���   � Title$ 锁屏窗口��$
��1080��$
��1920��$��0q� � Cursor$ Arrow�� � WindowState$ 	Maximized=� � 	Resources  �� MergedDictionaries �� Source��  �� $ Flipper/FlipperStyle.xaml?��� $ MaterialDictionary.xaml?�%    .��(� .��� $
��1080��$
��1920��.+�#  MaterialDesignThemes.Wpf.Card  $��0,200q�$	��500��$	��680��+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�.���� 	 �� Orientation$	 Vertical=�.+��� $��90��$	��200��$	 
Horizontal=�$��Center=�.+��� $��24(�$��当前用户：��$��Center=�
�������$��Center=��� $��24(�$��Center=�
�������$��Center=����� *	account+   DispatchApp.wpf.Flipper  -   
  Name$
 flipc��$	��480��$��Center=�$��Center=�$	��410��$��46,20,44,100q�   FrontContent � $	��360��$	��200�� � RowDefinitions �� $	��260 ��� $��60 ��� $	��20* �.+��� .+��� $��0q�$	��260��
������    FlipCommand#��Z $��0 4 0 20q�.���� .+��� $"��Resources/lock_close_128.png���� 
�������$��Center=�$��Bottom=�$��Microsoft YaHei)�$��24(�$��解锁���� $��1��
�������$��Center=�$	��Top=�$��Microsoft YaHei)�$��24(�$��正在锁定中��   BackContent � $	��380��$	��260�� �� $��30 ��� $��50 ��� $��50 ��� $	��76* �.+��� $��24(�$��解锁密码：��
�������G� -   $��
TxPassword��$��1��
�������$��Microsoft YaHei)�$��Center=�$��0,0,0,2q�$��20(��� -    ��Name$ message��$��2��$��24(�$�� ��
�������$
��10,0q��� $��3��$	 
Horizontal=�.+��� -   $��24(�$��确定�� �� 	IsDefault . $	��100��$
��20,0q�$��Center=�$��Center=��� $��24(�$��取消��$	��100��$��Center=�$��Center=�#��Z 
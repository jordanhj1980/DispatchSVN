   M S B A M L   `   `   `  ����F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DispatchApp.wpfDispatchApp.wpf  5
   	       DispatchApp.LockScreen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   )localclr-namespace:DispatchApp.wpf  5
   	     � WindowStyle$
  None=�5   	    � AllowsTransparency .    MaterialDesignPaper#���   5   	     MaterialDesignFont#���  5   	    � Title$ 锁屏窗口��5   	   $
��1080��6   $
��1920��6$   $��0q�5	   	    � Cursor$ Arrow��6    � WindowState$ 	Maximized=�6#    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Flipper/FlipperStyle.xaml?�6%   �� 5      $ MaterialDictionary.xaml?�6%   %    5      .��5      (� .��5   
   � $
��1080��6   $
��1920��6   .+�5      #  MaterialDesignThemes.Wpf.Card  $��0,200q�6"   $	��500��61   $	��680��6=   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�6J   .��5      �� 	 �� Orientation$	 Vertical=�6   .+�5      �� $��90��6!   $	��200��6-   $	 
Horizontal=�69   $��Center=�6R   .+�5      �� $��24(�6%   $��当前用户：��63   $��Center=�5       
�������$��Center=�6N   �� 5      $��24(�6$   $��Center=�5       
�������$��Center=�6N   ��5   2   �� *	account+5   N   5         DispatchApp.wpf.Flipper  5      -   5        
  Name$
 flipc��5   $   $	��480��63   $��Center=�6@   $��Center=�6]   $	��410��6x   $��46,20,44,100q�6�      FrontContent 5       � 5!      $	��360��6#   $	��200��60    � RowDefinitions 5"   "   �� 5#   &   $	��260 �64   �� 5$   &   $��60 �64   �� 5%   &   $	��20* �64   .+�5(   "   �� .+�5)   &   �� $��0q�6-   $	��260��6A   
������    FlipCommand#��Z 6g   $��0 4 0 20q�6�   .��5*   *   �� .+�5+   .   �� $"��Resources/lock_close_128.png��64   �� 5,   .   
�������$��Center=�6K   $��Bottom=�6h   $��Microsoft YaHei)�6�   $��24(�6�   $��解锁��6�   5-   +   5.   '   5/   #   �� 50   "   $��1��6,   
�������$��Center=�6L   $	��Top=�6i   $��Microsoft YaHei)�6�   $��24(�6�   $��正在锁定中��6�   51         BackContent 53      � 54      $	��380��6#   $	��260��60    55   "   �� 56   &   $��30 �64   �� 57   &   $��50 �64   �� 58   &   $��50 �64   �� 59   &   $	��76* �64   .+�5;   "   �� $��24(�6,   $��解锁密码：��6:   
�������6G   G� 5<   "   -   5        $��
TxPassword��5<   ;   $��1��6.   
�������$��Microsoft YaHei)�6e   $��Center=�5=      $��0,0,0,2q�60   $��20(�6A   �� 5>   "   -   5         ��Name$ message��5>   9   $��2��6,   $��24(�6J   $�� ��6X   
�������$
��10,0q�6s   �� 5@   "   $��3��6-   $	 
Horizontal=�6:   .+�5A   &   �� -   5        $��24(�5A   -   $��确定��6;    �� 	IsDefault . $	��100��6Y   $
��20,0q�6e   $��Center=�6s   $��Center=�6�   6�   �� 5B   &   $��24(�6-   $��取消��6;   $	��100��6H   $��Center=�6T   $��Center=�6o   #��Z 6�   5C   #   5D      5F      5G      5H      5I      5J      5K      
   M S B A M L   `   `   `  ����F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp  5          DispatchApp.UserCall   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _Microsoft.Expression.Drawing, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing  5      %localclr-namespace:DispatchApp  5      -   5          >�Name$  usname��5   M    � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $D >/DispatchApp;component/Client/UserlabelResourceDictionary.xaml?�6%   %    5      .��5      (� .��5   
   �  5         DispatchApp.DateConverter   cvtDate    DispatchApp.ImageConverter  cvtImage��%:   &	        &	        5      6&     5      6'   5      .+�5      �� $	��150��6   .+�5      �� -   5         ��Name$ 
ButtonBack��5      $	��0,2q�6)     UserRoundedButton#���  5      
�������$
��Left=�5      $	��146��5      $��Center=�5!      $	��186��5"      ���#f1f4ff.��5$      � $	��141��6   $	��186��6(    � RowDefinitions 5%      �� 5&      $	��24* �6(   �� 5'      $	��23* �6(    � ColumnDefinitions 5)      �� 5*      $
��123* �6+   �� 5+      $
��130* �6+   .+�5.      �� -   5         ��Name$ labelPoleId��5.      $
��Left=�61   $	��177��6L   $��2��6X     UserRoundedLabel#���  50      $��0,2,0,54.333q�51      $��2��6.   ��5/      ��  �� Path$ CurrentState�	 �� 	Converter#	 [  .��53      � $	��Top=�6   $��64��67   .+�54      �� -   5        $ labelNumFromId��54   $   $
��Left=�56       $��Center=�57       $��Right=�58       $	��Top=�6<   $	��157��6T   $��40��6`   
����������	#FF359F28$��22(�6�   $��0,-10,0.333,0q�6�   ��55   $   �� *
NameFromId+��58      Z� 6�   �� 5;      -   5        $ labelNumToId��5;   $   $��Right=�5=   $   $��Center=�6G   $��Right=�6i   $	��Top=�6�   $	��157��6�   $��40��6�   
�������$��0,30,0.333,-6q�6�   $��22(�6�   ��5<   $   �� *
NameToId+��5=   �   Z� 6�   5>      5?      �� 5B      -   5        
 ��Name$
 
PhoneState��5B       J� RenderTransformOrigin$ 
1.062,2.37&�6z   $��14,20,12.667,10.333q�6�   $��0��6�   $��1��6�   ��60   �� $ CurrentState�#	 [ 6�   �� 5D      -   5        $
��Time��5D      $��Center=�6*   $��1.333,20,9,10.333q�6N   $��1��6i   
���   �$��24(�6�   $��1��6�   ��6y   Z� ��6�   Z� 6�   5I      5J      5K      5L      5M      5N      
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class MenuViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _menuBarCode = """
                                  <Border HorizontalAlignment="Center" CornerRadius="{DynamicResource MediumCornerRadius}"
                                          BorderBrush="{DynamicResource BorderColor}" BorderThickness="1"
                                          BoxShadow="{DynamicResource Shadow}" Padding="4" Height="36">
                                      <Menu>
                                          <MenuItem Header="File">
                                              <MenuItem Header="New Tab" InputGesture="Ctrl + T" />
                                              <MenuItem Header="New Window" InputGesture="Ctrl + N" />
                                              <MenuItem Header="New Incognito Window" IsEnabled="False" />
                                              <Separator />
                                              <MenuItem Header="Share">
                                                  <MenuItem Header="Email link" Width="128" />
                                                  <MenuItem Header="Messages" />
                                                  <MenuItem Header="Notes" />
                                              </MenuItem>
                                              <Separator />
                                              <MenuItem Header="Print" InputGesture="Ctrl + P" />
                                          </MenuItem>
                                          <MenuItem Header="Edit">
                                              <MenuItem Header="Undo" InputGesture="Ctrl + Z" Width="192" />
                                              <MenuItem Header="Redo" InputGesture="Ctrl + Shift + Z" />
                                              <Separator />
                                              <MenuItem Header="Find">
                                                  <MenuItem Header="Search the web" />
                                                  <MenuItem Header="Find..." />
                                                  <MenuItem Header="Find Next" />
                                                  <MenuItem Header="Find Previous" />
                                              </MenuItem>
                                              <Separator />
                                              <MenuItem Header="Cut" InputGesture="Ctrl + X" />
                                              <MenuItem Header="Copy" InputGesture="Ctrl + C" />
                                              <MenuItem Header="Paste" InputGesture="Ctrl + V" />
                                          </MenuItem>
                                          <MenuItem Header="View">
                                              <MenuItem Header="Always Show Bookmarks Bar" />
                                              <MenuItem Header="Always Show Full URLs">
                                                  <MenuItem.Icon>
                                                      <icons:Lucide Icon="Check" Width="12"
                                                                    StrokeBrush="{DynamicResource ForegroundColor}" />
                                                  </MenuItem.Icon>
                                              </MenuItem>
                                              <Separator />
                                              <MenuItem Header="Reload" InputGesture="Ctrl + R" />
                                              <MenuItem Header="Force Reload" InputGesture="Ctrl + Shift + R" IsEnabled="False" />
                                              <Separator />
                                              <MenuItem Header="Toggle Fullscreen" InputGesture="F11" />
                                              <Separator />
                                              <MenuItem Header="Hide Sidebar" />
                                          </MenuItem>
                                          <MenuItem Header="Profiles">
                                              <MenuItem Header="Andy" Width="192" />
                                              <MenuItem Header="Benoit">
                                                  <MenuItem.Icon>
                                                      <Border CornerRadius="{DynamicResource RoundedCornerRadius}" Width="12"
                                                              Height="12" Background="{DynamicResource ForegroundColor}" />
                                                  </MenuItem.Icon>
                                              </MenuItem>
                                              <MenuItem Header="Luis" />
                                              <Separator />
                                              <MenuItem Header="Edit..." />
                                              <Separator />
                                              <MenuItem Header="Add Profile..." />
                                          </MenuItem>
                                      </Menu>
                                  </Border>
                                  """;

    [ObservableProperty]
    private string _dropDownCode = """
                                   <StackPanel Orientation="Horizontal" Spacing="16" VerticalAlignment="Center"
                                               HorizontalAlignment="Center">
                                       <StackPanel.Resources>
                                       </StackPanel.Resources>
                                       <Menu>
                                           <MenuItem Margin="12,0" Classes="Primary" Header="Primary"
                                                     extensions:MenuItem.PopupPlacement="Bottom">
                                               <extensions:MenuItem.Label>
                                                   <TextBlock Text="My Account" FontSize="16" FontWeight="SemiBold" />
                                               </extensions:MenuItem.Label>
                                               <MenuItem Header="Profile" InputGesture="Ctrl + Shift + P" Width="224" />
                                               <MenuItem Header="Billing" InputGesture="Ctrl + B" />
                                               <MenuItem Header="Settings" InputGesture="Ctrl + S" />
                                               <MenuItem Header="Keyboard shortcuts" InputGesture="Ctrl + K" />
                                               <Separator />
                                               <MenuItem Header="Team" />
                                               <MenuItem Header="Invite users">
                                                   <MenuItem Header="Email" Width="128" />
                                                   <MenuItem Header="Message" />
                                                   <MenuItem Header="More..." />
                                               </MenuItem>
                                               <MenuItem Header="New Team" InputGesture="Ctrl + T" />
                                               <Separator />
                                               <MenuItem Header="Github" />
                                               <MenuItem Header="Support" />
                                               <MenuItem Header="API" IsEnabled="False" />
                                               <Separator />
                                               <MenuItem Header="Log out" InputGesture="Ctrl + Shift + Q" />
                                               <Separator />
                                               <MenuItem Classes="Destructive" Header="Delete Account" />
                                           </MenuItem>
                                           <MenuItem Margin="12,0" Classes="Secondary" Header="Secondary"
                                                     extensions:MenuItem.PopupPlacement="Bottom">
                                               <extensions:MenuItem.Label>
                                                   <TextBlock Text="My Account" FontSize="16" FontWeight="SemiBold" />
                                               </extensions:MenuItem.Label>
                                               <MenuItem Header="Profile" InputGesture="Ctrl + Shift + P" Width="224" />
                                               <MenuItem Header="Billing" InputGesture="Ctrl + B" />
                                               <MenuItem Header="Settings" InputGesture="Ctrl + S" />
                                               <MenuItem Header="Keyboard shortcuts" InputGesture="Ctrl + K" />
                                               <Separator />
                                               <MenuItem Header="Team" />
                                               <MenuItem Header="Invite users">
                                                   <MenuItem Header="Email" Width="128" />
                                                   <MenuItem Header="Message" />
                                                   <MenuItem Header="More..." />
                                               </MenuItem>
                                               <MenuItem Header="New Team" InputGesture="Ctrl + T" />
                                               <Separator />
                                               <MenuItem Header="Github" />
                                               <MenuItem Header="Support" />
                                               <MenuItem Header="API" IsEnabled="False" />
                                               <Separator />
                                               <MenuItem Header="Log out" InputGesture="Ctrl + Shift + Q" />
                                               <Separator />
                                               <MenuItem Classes="Destructive" Header="Delete Account" />
                                           </MenuItem>
                                           <MenuItem Margin="12,0" Classes="Outline" Header="Outline"
                                                     extensions:MenuItem.PopupPlacement="Bottom">
                                               <extensions:MenuItem.Label>
                                                   <TextBlock Text="My Account" FontSize="16" FontWeight="SemiBold" />
                                               </extensions:MenuItem.Label>
                                               <MenuItem Header="Profile" InputGesture="Ctrl + Shift + P" Width="224" />
                                               <MenuItem Header="Billing" InputGesture="Ctrl + B" />
                                               <MenuItem Header="Settings" InputGesture="Ctrl + S" />
                                               <MenuItem Header="Keyboard shortcuts" InputGesture="Ctrl + K" />
                                               <Separator />
                                               <MenuItem Header="Team" />
                                               <MenuItem Header="Invite users">
                                                   <MenuItem Header="Email" Width="128" />
                                                   <MenuItem Header="Message" />
                                                   <MenuItem Header="More..." />
                                               </MenuItem>
                                               <MenuItem Header="New Team" InputGesture="Ctrl + T" />
                                               <Separator />
                                               <MenuItem Header="Github" />
                                               <MenuItem Header="Support" />
                                               <MenuItem Header="API" IsEnabled="False" />
                                               <Separator />
                                               <MenuItem Header="Log out" InputGesture="Ctrl + Shift + Q" />
                                               <Separator />
                                               <MenuItem Classes="Destructive" Header="Delete Account" />
                                           </MenuItem>
                                           <MenuItem Margin="12,0" Classes="Ghost" Header="Ghost"
                                                     extensions:MenuItem.PopupPlacement="Bottom">
                                               <extensions:MenuItem.Label>
                                                   <TextBlock Text="My Account" FontSize="16" FontWeight="SemiBold" />
                                               </extensions:MenuItem.Label>
                                               <MenuItem Header="Profile" InputGesture="Ctrl + Shift + P" Width="224" />
                                               <MenuItem Header="Billing" InputGesture="Ctrl + B" />
                                               <MenuItem Header="Settings" InputGesture="Ctrl + S" />
                                               <MenuItem Header="Keyboard shortcuts" InputGesture="Ctrl + K" />
                                               <Separator />
                                               <MenuItem Header="Team" />
                                               <MenuItem Header="Invite users">
                                                   <MenuItem Header="Email" Width="128" />
                                                   <MenuItem Header="Message" />
                                                   <MenuItem Header="More..." />
                                               </MenuItem>
                                               <MenuItem Header="New Team" InputGesture="Ctrl + T" />
                                               <Separator />
                                               <MenuItem Header="Github" />
                                               <MenuItem Header="Support" />
                                               <MenuItem Header="API" IsEnabled="False" />
                                               <Separator />
                                               <MenuItem Header="Log out" InputGesture="Ctrl + Shift + Q" />
                                               <Separator />
                                               <MenuItem Classes="Destructive" Header="Delete Account" />
                                           </MenuItem>
                                           <MenuItem Margin="12,0" Classes="Icon"
                                                     extensions:MenuItem.PopupPlacement="Bottom">
                                               <MenuItem.Header>
                                                   <icons:Lucide Icon="Ellipsis" StrokeBrush="{DynamicResource ForegroundColor}"
                                                                 Width="16" />
                                               </MenuItem.Header>
                                               <extensions:MenuItem.Label>
                                                   <TextBlock Text="My Account" FontSize="16" FontWeight="SemiBold" />
                                               </extensions:MenuItem.Label>
                                               <MenuItem Header="Profile" InputGesture="Ctrl + Shift + P" Width="224" />
                                               <MenuItem Header="Billing" InputGesture="Ctrl + B" />
                                               <MenuItem Header="Settings" InputGesture="Ctrl + S" />
                                               <MenuItem Header="Keyboard shortcuts" InputGesture="Ctrl + K" />
                                               <Separator />
                                               <MenuItem Header="Team" />
                                               <MenuItem Header="Invite users">
                                                   <MenuItem Header="Email" Width="128" />
                                                   <MenuItem Header="Message" />
                                                   <MenuItem Header="More..." />
                                               </MenuItem>
                                               <MenuItem Header="New Team" InputGesture="Ctrl + T" />
                                               <Separator />
                                               <MenuItem Header="Github" />
                                               <MenuItem Header="Support" />
                                               <MenuItem Header="API" IsEnabled="False" />
                                               <Separator />
                                               <MenuItem Header="Log out" InputGesture="Ctrl + Shift + Q" />
                                               <Separator />
                                               <MenuItem Classes="Destructive" Header="Delete Account" />
                                           </MenuItem>
                                       </Menu>
                                   </StackPanel>
                                   """;
}
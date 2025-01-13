using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ButtonsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _primaryCode = """
                                  <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                      <Button Classes="Primary" Content="Primary" />
                                      <Button Classes="Primary" Content="Edit">
                                          <extensions:ButtonExt.Icon>
                                              <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                            StrokeBrush="{DynamicResource ForegroundColor}" />
                                          </extensions:ButtonExt.Icon>
                                      </Button>
                                      <Button Classes="Primary" extensions:ButtonExt.ShowProgress="True" Content="Saving...">
                                          <extensions:ButtonExt.Icon>
                                              <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                            StrokeBrush="{DynamicResource ForegroundColor}" />
                                          </extensions:ButtonExt.Icon>
                                      </Button>
                                      <Button Classes="Primary" IsEnabled="False" Content="Disable" />
                                  </StackPanel>
                                  """;

    [ObservableProperty]
    private string _secondaryCode = """
                                    <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                        <Button Classes="Secondary" Content="Secondary" />
                                        <Button Classes="Secondary" Content="Edit">
                                            <extensions:ButtonExt.Icon>
                                                <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                              StrokeBrush="{DynamicResource ForegroundColor}" />
                                            </extensions:ButtonExt.Icon>
                                        </Button>
                                        <Button Classes="Secondary" extensions:ButtonExt.ShowProgress="True" Content="Saving...">
                                            <extensions:ButtonExt.Icon>
                                                <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                              StrokeBrush="{DynamicResource ForegroundColor}" />
                                            </extensions:ButtonExt.Icon>
                                        </Button>
                                        <Button Classes="Secondary" IsEnabled="False" Content="Disable" />
                                    </StackPanel>
                                    """;

    [ObservableProperty]
    private string _destructiveCode = """
                                      <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                          <Button Classes="Destructive" Content="Destructive" />
                                          <Button Classes="Destructive" Content="Delete">
                                              <extensions:ButtonExt.Icon>
                                                  <icons:Lucide Icon="Trash" StrokeThickness="1.5" Width="10" Height="16"
                                                                StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                              </extensions:ButtonExt.Icon>
                                          </Button>
                                          <Button Classes="Destructive" extensions:ButtonExt.ShowProgress="True" Content="Saving...">
                                              <extensions:ButtonExt.Icon>
                                                  <icons:Lucide Icon="Trash" StrokeThickness="1.5" Width="10" Height="16"
                                                                StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                              </extensions:ButtonExt.Icon>
                                          </Button>
                                          <Button Classes="Destructive" IsEnabled="False" Content="Disable" />
                                      </StackPanel>
                                      """;

    [ObservableProperty]
    private string _outlineCode = """
                                  <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                      <Button Classes="Outline" Content="Outline" />
                                      <Button Classes="Outline" Content="Edit">
                                          <extensions:ButtonExt.Icon>
                                              <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                            StrokeBrush="{DynamicResource ForegroundColor}" />
                                          </extensions:ButtonExt.Icon>
                                      </Button>
                                      <Button Classes="Outline" extensions:ButtonExt.ShowProgress="True" Content="Saving...">
                                          <extensions:ButtonExt.Icon>
                                              <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                            StrokeBrush="{DynamicResource ForegroundColor}" />
                                          </extensions:ButtonExt.Icon>
                                      </Button>
                                      <Button Classes="Outline" IsEnabled="False" Content="Disable" />
                                  </StackPanel>
                                  """;

    [ObservableProperty]
    private string _ghostCode = """
                                <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                    <Button Classes="Ghost" Content="Ghost" />
                                    <Button Classes="Ghost" Content="Edit">
                                        <extensions:ButtonExt.Icon>
                                            <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                          StrokeBrush="{DynamicResource ForegroundColor}" />
                                        </extensions:ButtonExt.Icon>
                                    </Button>
                                    <Button Classes="Ghost" extensions:ButtonExt.ShowProgress="True" Content="Saving...">
                                        <extensions:ButtonExt.Icon>
                                            <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="10" Height="16"
                                                          StrokeBrush="{DynamicResource ForegroundColor}" />
                                        </extensions:ButtonExt.Icon>
                                    </Button>
                                    <Button Classes="Ghost" IsEnabled="False" Content="Disable" />
                                </StackPanel>
                                """;

    [ObservableProperty]
    private string _iconCode = """
                               <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                   <Button Classes="Icon" BorderBrush="{DynamicResource BorderColor}">
                                       <extensions:ButtonExt.Icon>
                                           <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                         StrokeBrush="{DynamicResource ForegroundColor}" />
                                       </extensions:ButtonExt.Icon>
                                   </Button>
                                   <Button Classes="Icon" extensions:ButtonExt.ShowProgress="True"
                                           BorderBrush="{DynamicResource BorderColor}">
                                       <extensions:ButtonExt.Icon>
                                           <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                         StrokeBrush="{DynamicResource ForegroundColor}" />
                                       </extensions:ButtonExt.Icon>
                                   </Button>
                                   <Button Classes="Icon" IsEnabled="False" BorderBrush="{DynamicResource BorderColor}">
                                       <extensions:ButtonExt.Icon>
                                           <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                         StrokeBrush="{DynamicResource ForegroundColor}" />
                                       </extensions:ButtonExt.Icon>
                                   </Button>
                               </StackPanel>
                               """;

    [ObservableProperty]
    private string _destructiveIconCode = """
                                          <StackPanel Spacing="8" Orientation="Horizontal" HorizontalAlignment="Center">
                                              <Button Classes="DestructiveIcon">
                                                  <extensions:ButtonExt.Icon>
                                                      <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                                    StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                                  </extensions:ButtonExt.Icon>
                                              </Button>
                                              <Button Classes="DestructiveIcon" extensions:ButtonExt.ShowProgress="True">
                                                  <extensions:ButtonExt.Icon>
                                                      <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                                    StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                                  </extensions:ButtonExt.Icon>
                                              </Button>
                                              <Button Classes="DestructiveIcon" IsEnabled="False">
                                                  <extensions:ButtonExt.Icon>
                                                      <icons:Lucide Icon="Pencil" StrokeThickness="1.5" Width="16"
                                                                    StrokeBrush="{DynamicResource DestructiveForegroundColor}" />
                                                  </extensions:ButtonExt.Icon>
                                              </Button>
                                          </StackPanel>
                                          """;
}
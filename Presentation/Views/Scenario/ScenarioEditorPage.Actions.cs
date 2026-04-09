using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;
using ScenarioModel = EWTSS_DESKTOP.Core.Models.Scenario;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage
    {
        private void CreateScenario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                TimeSpan.TryParse(ScenarioExecutionTimeTextBox.Text, out var executeTime);

                var scenario = new ScenarioModel
                {
                    Name = ScenarioNameTextBox.Text?.Trim(),
                    Description = ScenarioDescriptionTextBox.Text?.Trim(),
                    StartDate = DateTime.Now.Date,
                    StartTime = DateTime.Now.TimeOfDay,
                    ExecuteDate = DateTime.Now.Date,
                    ExecuteTime = executeTime,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = true,
                    UserId = 1
                };

                db.Scenarios.Add(scenario);
                db.SaveChanges();

                MessageBox.Show("Scenario created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create scenario.\n\n{ex.Message}");
            }
        }

        private void UpdateScenario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                var scenario = db.Scenarios.FirstOrDefault(x => x.Id == _scenarioId);
                if (scenario == null)
                {
                    MessageBox.Show("Scenario not found.");
                    return;
                }

                scenario.Name = ScenarioNameTextBox.Text?.Trim();
                scenario.Description = ScenarioDescriptionTextBox.Text?.Trim();
                scenario.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                ScenarioTitleTextBlock.Text = $"{scenario.Name} | - TREE STRUCTURE";
                MessageBox.Show("Scenario updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update scenario.\n\n{ex.Message}");
            }
        }

        private void CreateAreaOperation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int scenarioId = _scenarioId > 0
                    ? _scenarioId
                    : db.Scenarios.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

                if (scenarioId <= 0)
                {
                    MessageBox.Show("Please create a scenario first.");
                    return;
                }

                AreaOperation areaOperation;
                bool isUpdate = false;

                if (_selectedAreaOperationId.HasValue)
                {
                    areaOperation = db.AreaOperations.FirstOrDefault(x => x.Id == _selectedAreaOperationId.Value);
                    if (areaOperation == null)
                    {
                        MessageBox.Show("Selected Area Of Operation not found.");
                        return;
                    }

                    isUpdate = true;
                }
                else
                {
                    areaOperation = db.AreaOperations.FirstOrDefault(x => x.ScenarioId == scenarioId);

                    if (areaOperation == null)
                    {
                        areaOperation = new AreaOperation
                        {
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            IsActive = true,
                            ScenarioId = scenarioId
                        };

                        db.AreaOperations.Add(areaOperation);
                    }
                    else
                    {
                        isUpdate = true;
                    }
                }

                areaOperation.Name = "AREA OF OPERATION";
                areaOperation.Description = "Created from Scenario Editor";
                areaOperation.Altitude = "1000";
                areaOperation.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                _selectedAreaOperationId = areaOperation.Id;
                SetAreaOperationButtonState(true);

                MessageBox.Show(isUpdate
                    ? "Area Of Operation updated successfully."
                    : "Area Of Operation created successfully.");

                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save Area Of Operation");
            }
        }

        private void CreateOrUpdateAreaOperation_Click(object sender, RoutedEventArgs e)
        {
            CreateAreaOperation_Click(sender, e);
        }

        private void CreateCc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int blueLineId = db.ScenarioLines
                    .Where(x => x.AreaOperation.ScenarioId == _scenarioId && x.Name == "BLUE LINE")
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (blueLineId <= 0)
                {
                    MessageBox.Show("BLUE LINE not found for this scenario.");
                    return;
                }

                var cc = new Cc
                {
                    CcName = string.IsNullOrWhiteSpace(CcNameTextBox.Text) ? "CC1" : CcNameTextBox.Text.Trim(),
                    Latitude = string.IsNullOrWhiteSpace(CcLatitudeTextBox.Text) ? "0" : CcLatitudeTextBox.Text.Trim(),
                    Longitude = string.IsNullOrWhiteSpace(CcLongitudeTextBox.Text) ? "0" : CcLongitudeTextBox.Text.Trim(),
                    LineId = blueLineId
                };

                db.Ccs.Add(cc);
                db.SaveChanges();

                MessageBox.Show("CC created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create CC.\n\n{ex.Message}");
            }
        }

        // keep remaining create/update methods here:
        private void CreateRdfs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int ccId = _selectedCcId ?? db.Ccs
                    .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId)
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (ccId <= 0)
                {
                    MessageBox.Show("Please create CC first before creating RDFS.");
                    return;
                }

                if (!TryParseDouble(RdfsMinFreqTextBox.Text, out double minFreq) ||
                    !TryParseDouble(RdfsMaxFreqTextBox.Text, out double maxFreq))
                {
                    MessageBox.Show("Please enter valid RDFS frequency values.");
                    return;
                }

                int? antennaHeight = null;
                if (int.TryParse(RdfsAntennaHeightTextBox.Text, out int parsedHeight))
                    antennaHeight = parsedHeight;

                Entity entity;
                if (_selectedEntityId.HasValue)
                {
                    entity = db.Entities.FirstOrDefault(x => x.Id == _selectedEntityId.Value);
                    if (entity == null)
                    {
                        MessageBox.Show("Selected RDFS not found.");
                        return;
                    }
                }
                else
                {
                    entity = new Entity();
                    db.Entities.Add(entity);
                }

                entity.Name = string.IsNullOrWhiteSpace(RdfsNameTextBox.Text) ? "RDFS1" : RdfsNameTextBox.Text.Trim();
                entity.StartFrequencyValue = minFreq;
                entity.StopFrequencyValue = maxFreq;
                entity.AntennaType = string.IsNullOrWhiteSpace(RdfsAntennaTypeTextBox.Text) ? "Default" : RdfsAntennaTypeTextBox.Text.Trim();
                entity.Polarization = string.IsNullOrWhiteSpace(RdfsPolarizationTextBox.Text) ? "Vertical" : RdfsPolarizationTextBox.Text.Trim();
                entity.AntennaHeight = antennaHeight;
                entity.ScanType = string.IsNullOrWhiteSpace(RdfsScanTypeTextBox.Text) ? "SCAN" : RdfsScanTypeTextBox.Text.Trim();
                entity.CcId = ccId;
                entity.EntityType = EntityType.RDFS;

                db.SaveChanges();

                MessageBox.Show(_selectedEntityId.HasValue ? "RDFS updated successfully." : "RDFS saved successfully.");
                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save RDFS");
            }
        }
        private void CreateJsvushf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int ccId = _selectedCcId ?? db.Ccs
                    .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId)
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (ccId <= 0)
                {
                    MessageBox.Show("Please create CC first before creating JSVUSHF.");
                    return;
                }

                if (!TryParseDouble(JsvushfMinFreqTextBox.Text, out double minFreq) ||
                    !TryParseDouble(JsvushfMaxFreqTextBox.Text, out double maxFreq))
                {
                    MessageBox.Show("Please enter valid frequency values.");
                    return;
                }

                int? antennaHeight = null;
                if (int.TryParse(JsvushfAntennaHeightTextBox.Text, out int parsedHeight))
                    antennaHeight = parsedHeight;

                Entity entity;
                if (_selectedEntityId.HasValue)
                {
                    entity = db.Entities.FirstOrDefault(x => x.Id == _selectedEntityId.Value);
                    if (entity == null)
                    {
                        MessageBox.Show("Selected JSVUSHF not found.");
                        return;
                    }
                }
                else
                {
                    entity = new Entity();
                    db.Entities.Add(entity);
                }

                entity.Name = string.IsNullOrWhiteSpace(JsvushfNameTextBox.Text) ? "JSVUSHF1" : JsvushfNameTextBox.Text.Trim();
                entity.StartFrequencyValue = minFreq;
                entity.StopFrequencyValue = maxFreq;
                entity.AntennaType = string.IsNullOrWhiteSpace(JsvushfAntennaTypeTextBox.Text) ? "Default" : JsvushfAntennaTypeTextBox.Text.Trim();
                entity.Polarization = string.IsNullOrWhiteSpace(JsvushfPolarizationTextBox.Text) ? "Vertical" : JsvushfPolarizationTextBox.Text.Trim();
                entity.AntennaHeight = antennaHeight;
                entity.ScanType = string.IsNullOrWhiteSpace(JsvushfScanTypeTextBox.Text) ? "SCAN" : JsvushfScanTypeTextBox.Text.Trim();
                entity.CcId = ccId;
                entity.EntityType = EntityType.JSVUSHF;

                db.SaveChanges();

                MessageBox.Show(_selectedEntityId.HasValue ? "JSVUSHF updated successfully." : "JSVUSHF saved successfully.");
                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save JSVUSHF");
            }
        }
        private void CreateCommEmitterAllied_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int blueLineId = db.ScenarioLines
                    .Where(x => x.AreaOperation.ScenarioId == _scenarioId && x.Name == "BLUE LINE")
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (blueLineId <= 0)
                {
                    MessageBox.Show("BLUE LINE not found for this scenario.");
                    return;
                }

                double? power = null;
                if (double.TryParse(CommEmitterPowerTextBox.Text, out double parsedPower))
                    power = parsedPower;

                double? frequency = null;
                if (double.TryParse(CommEmitterFrequencyTextBox.Text, out double parsedFrequency))
                    frequency = parsedFrequency;

                double? bandwidth = null;
                if (double.TryParse(CommEmitterBandwidthTextBox.Text, out double parsedBandwidth))
                    bandwidth = parsedBandwidth;

                double? gain = null;
                if (double.TryParse(CommEmitterGainTextBox.Text, out double parsedGain))
                    gain = parsedGain;

                double? scanRate = null;
                if (double.TryParse(CommEmitterScanRateTextBox.Text, out double parsedScanRate))
                    scanRate = parsedScanRate;

                Emitter emitter;
                if (_selectedEmitterId.HasValue)
                {
                    emitter = db.Emitters.FirstOrDefault(x => x.Id == _selectedEmitterId.Value);
                    if (emitter == null)
                    {
                        MessageBox.Show("Selected COMMEMITTERALLIED not found.");
                        return;
                    }
                }
                else
                {
                    emitter = new Emitter
                    {
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };
                    db.Emitters.Add(emitter);
                }

                emitter.Name = string.IsNullOrWhiteSpace(CommEmitterNameTextBox.Text)
                    ? "COMMEMITTERALLIED1"
                    : CommEmitterNameTextBox.Text.Trim();

                emitter.PlatformType = PlatformType.LAND_STATIC;
                emitter.ModeType = (CommEmitterModeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "FF";
                emitter.PowerTransmitted = power;
                emitter.StartFrequencyValue = frequency;
                emitter.StopFrequencyValue = frequency;
                emitter.Bandwidth = bandwidth;
                emitter.HopPeriodValue = 0;
                emitter.HopPeriodUnit = "ms";
                emitter.HopInterPeriodValue = 0;
                emitter.HopInterPeriodUnit = "ms";
                emitter.ModulationType = "AM";
                emitter.PatternType = "DEFAULT";
                emitter.ScanRate = scanRate;
                emitter.AntennaType = string.IsNullOrWhiteSpace(CommEmitterAntennaTypeTextBox.Text)
                    ? "DEFAULT"
                    : CommEmitterAntennaTypeTextBox.Text.Trim();
                emitter.Gain = gain;
                emitter.Polarization = string.IsNullOrWhiteSpace(CommEmitterPolarizationTextBox.Text)
                    ? "VERTICAL"
                    : CommEmitterPolarizationTextBox.Text.Trim();
                emitter.LineId = blueLineId;
                emitter.EmitterType = (CommEmitterEmitterTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "COMMUNICATION";
                emitter.Type = EmitterKeyName.COMMEMITTERALLIED;
                emitter.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                MessageBox.Show(_selectedEmitterId.HasValue
                    ? "COMMEMITTERALLIED updated successfully."
                    : "COMMEMITTERALLIED saved successfully.");

                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save COMMEMITTERALLIED");
            }
        }
            private void CreateRadarEmitterAllied_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int blueLineId = db.ScenarioLines
                    .Where(x => x.AreaOperation.ScenarioId == _scenarioId && x.Name == "BLUE LINE")
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (blueLineId <= 0)
                {
                    MessageBox.Show("BLUE LINE not found for this scenario.");
                    return;
                }

                double? power = null;
                if (double.TryParse(RadarEmitterPowerTextBox.Text, out double parsedPower))
                    power = parsedPower;

                double? frequency = null;
                if (double.TryParse(RadarEmitterFrequencyTextBox.Text, out double parsedFrequency))
                    frequency = parsedFrequency;

                double? bandwidth = null;
                if (double.TryParse(RadarEmitterBandwidthTextBox.Text, out double parsedBandwidth))
                    bandwidth = parsedBandwidth;

                double? gain = null;
                if (double.TryParse(RadarEmitterGainTextBox.Text, out double parsedGain))
                    gain = parsedGain;

                double? scanRate = null;
                if (double.TryParse(RadarEmitterScanRateTextBox.Text, out double parsedScanRate))
                    scanRate = parsedScanRate;

                Emitter emitter;
                if (_selectedEmitterId.HasValue)
                {
                    emitter = db.Emitters.FirstOrDefault(x => x.Id == _selectedEmitterId.Value);
                    if (emitter == null)
                    {
                        MessageBox.Show("Selected RADAREMITTERALLIED not found.");
                        return;
                    }
                }
                else
                {
                    emitter = new Emitter
                    {
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };
                    db.Emitters.Add(emitter);
                }

                emitter.Name = string.IsNullOrWhiteSpace(RadarEmitterNameTextBox.Text)
                    ? "RADAREMITTERALLIED1"
                    : RadarEmitterNameTextBox.Text.Trim();

                emitter.PlatformType = PlatformType.LAND_STATIC;
                emitter.ModeType = (RadarEmitterModeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "FF";
                emitter.PowerTransmitted = power;
                emitter.StartFrequencyValue = frequency;
                emitter.StopFrequencyValue = frequency;
                emitter.Bandwidth = bandwidth;
                emitter.HopPeriodValue = 0;
                emitter.HopPeriodUnit = "ms";
                emitter.HopInterPeriodValue = 0;
                emitter.HopInterPeriodUnit = "ms";
                emitter.ModulationType = "AM";
                emitter.PatternType = "DEFAULT";
                emitter.ScanRate = scanRate;
                emitter.AntennaType = string.IsNullOrWhiteSpace(RadarEmitterAntennaTypeTextBox.Text)
                    ? "DEFAULT"
                    : RadarEmitterAntennaTypeTextBox.Text.Trim();
                emitter.Gain = gain;
                emitter.Polarization = string.IsNullOrWhiteSpace(RadarEmitterPolarizationTextBox.Text)
                    ? "VERTICAL"
                    : RadarEmitterPolarizationTextBox.Text.Trim();
                emitter.LineId = blueLineId;
                emitter.EmitterType = (RadarEmitterEmitterTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "RADAR";
                emitter.Type = EmitterKeyName.RADAREMITTERALLIED;
                emitter.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                MessageBox.Show(_selectedEmitterId.HasValue
                    ? "RADAREMITTERALLIED updated successfully."
                    : "RADAREMITTERALLIED saved successfully.");

                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save RADAREMITTERALLIED");
            }
        }
        private void CreateCommEmitter_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Comm Emitter create clicked.");
        }

        private void CreateRadar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Radar create clicked.");
        }
    }
}
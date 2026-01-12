// Chart.js helper functions for Blazor integration

let chartInstances = {};

window.updateOrCreateChart = function (canvasId, chartType, labels, datasets, yAxisLabel) {
    console.log(`updateOrCreateChart called for ${canvasId}`, {
        chartType,
        labelsCount: labels ? labels.length : 0,
        datasetsCount: datasets ? datasets.length : 0,
        yAxisLabel
    });
    
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error('Canvas element not found:', canvasId);
        return;
    }

    const ctx = canvas.getContext('2d');

    // Destroy existing chart if it exists
    if (chartInstances[canvasId]) {
        console.log(`Destroying existing chart: ${canvasId}`);
        chartInstances[canvasId].destroy();
    }

    // Log the data we're about to chart
    if (datasets && datasets.length > 0) {
        datasets.forEach((dataset, index) => {
            console.log(`Dataset ${index} (${dataset.label}):`, {
                dataPoints: dataset.data ? dataset.data.length : 0,
                data: dataset.data
            });
        });
    }

    // Create new chart
    try {
        chartInstances[canvasId] = new Chart(ctx, {
            type: chartType,
            data: {
                labels: labels,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                plugins: {
                    legend: {
                        position: 'top',
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';
                                if (label) {
                                    label += ': ';
                                }
                                if (yAxisLabel && yAxisLabel.includes('$')) {
                                    label += '$' + context.parsed.y.toFixed(2);
                                } else {
                                    label += context.parsed.y;
                                }
                                return label;
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function (value) {
                                if (yAxisLabel && yAxisLabel.includes('$')) {
                                    return '$' + value.toFixed(2);
                                }
                                return value;
                            }
                        },
                        title: {
                            display: true,
                            text: yAxisLabel
                        }
                    },
                    x: {
                        title: {
                            display: true,
                            text: 'Time'
                        }
                    }
                }
            }
        });
        console.log(`Chart created successfully: ${canvasId}`);
    } catch (error) {
        console.error(`Error creating chart ${canvasId}:`, error);
    }
};

window.destroyChart = function (canvasId) {
    if (chartInstances[canvasId]) {
        chartInstances[canvasId].destroy();
        delete chartInstances[canvasId];
    }
};

window.destroyAllCharts = function () {
    for (let canvasId in chartInstances) {
        if (chartInstances.hasOwnProperty(canvasId)) {
            chartInstances[canvasId].destroy();
        }
    }
    chartInstances = {};
};

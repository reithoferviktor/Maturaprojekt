module.exports = {
  launch_options: {
    executablePath: 'C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe',
    args: ['--no-sandbox', '--disable-dev-shm-usage'],
    protocolTimeout: 240000,
  },
  pdf_options: {
    format: 'A4',
    margin: { top: '20mm', bottom: '20mm', left: '18mm', right: '18mm' },
    printBackground: true,
  },
};

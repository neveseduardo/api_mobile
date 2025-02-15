/**
 * Below are the colors that are used in the app. The colors are defined in the light and dark mode.
 * There are many other ways to style your app. For example, [Nativewind](https://www.nativewind.dev/), [Tamagui](https://tamagui.dev/), [unistyles](https://reactnativeunistyles.vercel.app), etc.
 */

const tintColorLight = '#0a7ea4';
const tintColorDark = '#fff';

export const Colors = {
	light: {
		text: '#11181C',
		background: '#fff',
		tint: tintColorLight,
		icon: '#687076',
		tabIconDefault: '#687076',
		tabIconSelected: tintColorLight,
	},
	dark: {
		text: '#ECEDEE',
		background: '#151718',
		tint: tintColorDark,
		icon: '#9BA1A6',
		tabIconDefault: '#9BA1A6',
		tabIconSelected: tintColorDark,
	},
	palet: {
		primaryBlue: {
			50: '#eff6ff',
			100: '#dbeafe',
			200: '#bfdbfe',
			300: '#93c5fd',
			400: '#60a5fa',
			500: '#3b82f6',
			600: '#2563eb',
			700: '#1d4ed8',
			800: '#1e40af',
			900: '#1e3a8a',
			950: '#172554'
		},
		primaryRed: {
			50: '#fef2f2',
			100: '#fee2e2',
			200: '#fecaca',
			300: '#fca5a5',
			400: '#f87171',
			500: '#ef4444',
			600: '#dc2626',
			700: '#b91c1c',
			800: '#991b1b',
			900: '#7f1d1d',
			950: '#450a0a'
		},
		primaryGreen: {
			50: '#f0fdf4',
			100: '#dcfce7',
			200: '#bbf7d0',
			300: '#86efac',
			400: '#4ade80',
			500: '#22c55e',
			600: '#16a34a',
			700: '#15803d',
			800: '#166534',
			900: '#14532d',
			950: '#052e16'
		},
	},
	classes: {
		primary: 'bg-primary-500',
		info: 'bg-blue-500',
		success: 'bg-green-500',
		warning: 'bg-yellow-500',
		danger: 'bg-red-500',
		default: 'bg-gray-800',
	}
};

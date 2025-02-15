import React, { useEffect } from 'react';
import { StyleSheet } from 'react-native';
import { ThemedView } from '@/components/ThemedView';
import { ThemedText } from '@/components/ThemedText';
import { router } from 'expo-router';

const SplashScreen = () => {
	const APPNAME = process.env.EXPO_PUBLIC_APP_NAME;

	useEffect(() => {
		setTimeout(() => {
			router.replace('/(auth)/login');
		}, 3000);
	}, []);

	return (
		<ThemedView darkColor="#fff" style={styles.container}>
			<ThemedText darkColor="#333">{APPNAME}</ThemedText>
		</ThemedView>
	);
};

const styles = StyleSheet.create({
	container: {
		flex: 1,
		justifyContent: 'center',
		alignItems: 'center',
	},
});

export default SplashScreen;
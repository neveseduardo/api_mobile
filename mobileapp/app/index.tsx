import { useEffect } from 'react';
import { Text, View } from 'react-native';
import { router } from 'expo-router';

const SplashScreen = () => {
	const APPNAME = process.env.EXPO_PUBLIC_APP_NAME;

	useEffect(() => {
		setTimeout(() => {
			router.replace('/(auth)/userlogin');
		}, 3000);
	}, []);

	return (
		<View className="bg-[#151718] flex-1 justify-center items-center">
			<Text className="text-white">{APPNAME}</Text>
		</View>
	);
};
export default SplashScreen;
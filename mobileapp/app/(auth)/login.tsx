import { useState } from 'react';
import { View, Text } from 'react-native';
import { Href, router } from 'expo-router';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import PasswordInput from '@/components/ui/PasswordInput';
import { ThemedText } from '../../components/ThemedText';
import Ionicons from '@expo/vector-icons/Ionicons';

export default function LoginScreen() {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');

	const handleLogin = () => {
		router.replace("/(tabs)" as Href);
	};

	return (
		<ThemedView className="flex-1 items-center p-8">
			<View className='flex flex-col gap-5 w-full'>
				<View className="w-full flex justify-center items-center h-[200px]">
					<View className='w-[70px] h-[70px] bg-primary-500 rounded-full flex justify-center items-center'>
						<Ionicons name="lock-closed-outline" size={32} className='text-gray-500 dark:text-white' />
					</View>
				</View>

				<View className="w-full flex flex-row gap-2 items-center">
					<ThemedText className='text-white text-2xl'>Autenticação</ThemedText>
				</View>

				<View className="w-full">
					<ThemedText className='text-white'>Insira os dados solicitados abaixo para autenticar no aplicativo.</ThemedText>
				</View>

				<View className="flex flex-col gap-4 w-full">
					<TextInput
						keyboardType="email-address"
						placeholder="Email"
						value={email}
						onChangeText={setEmail}
					/>
					<PasswordInput
						placeholder="Senha"
						value={password}
						onChangeText={setPassword}
						secureTextEntry
					/>

					<Button onPress={handleLogin} color='primary' className='w-full'>
						<Text className='text-white'>ENTRAR</Text>
					</Button>
					<Button onPress={() => router.push('/(auth)/register')} className='w-full'>
						<Text className='text-white'>REGISTRAR</Text>
					</Button>
				</View>
			</View>
		</ThemedView>
	);
}